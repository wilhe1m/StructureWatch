using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using wilhe1m.StructureWatch.Models;

namespace wilhe1m.StructureWatch.Services
{
    /// <summary>
    ///     config for eve SSO/Swagger
    /// </summary>
    public class SwaggerConfig
    {
        private IConfigurationSection configurationSection;

        public SwaggerConfig(IConfigurationSection configurationSection)
        {
            this.configurationSection = configurationSection;
            UserAgent = configurationSection["UserAgent"];
            ClientId = configurationSection["ClientId"];
            SecretKey = configurationSection["SecretKey"];
            AuthVersion = configurationSection["AuthVersion"];
            CallbackUrl = configurationSection["CallbackUrl"];
            DataSource = configurationSection["DataSource"];
            EsiUrl = configurationSection["EsiUrl"];
            SSOUrl = configurationSection["SSOUrl"];
        }

        public string UserAgent { get; set; }
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
        public string AuthVersion { get; set; } = "v2";
        public string CallbackUrl { get; set; }
        public string DataSource { get; set; }
        public string EsiUrl { get; set; }
        public string SSOUrl { get; set; }

        public string ClientKey => Convert.ToBase64String(Encoding.ASCII.GetBytes(ClientId + ":" + SecretKey));
    }

    /// <summary>
    ///     Pulls data from the EVE Swagger intereface and SSO sign in.
    /// </summary>
    public static class EVESwagger
    {
        private static SwaggerConfig Config;

        private static HttpClient Client { get; set; }

        public static void InitClient(SwaggerConfig config)
        {
            Config = config;

            Client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            Client.DefaultRequestHeaders.Add("X-User-Agent", config.UserAgent);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }


        /// <summary>
        ///     Gets Login url from the EVE SSO
        /// </summary>
        public static string GetAuthUrl(List<string> requestedScopes, string state, string challengeCode = null)
        {
            var code_challenge = "";
            if (!string.IsNullOrEmpty(challengeCode))
                using (var sha256 = SHA256.Create())
                {
                    var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(challengeCode)).TrimEnd('=')
                        .Replace('+', '-').Replace('/', '_');
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(base64));
                    code_challenge = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

                    code_challenge = $"&code_challenge={code_challenge}";
                }

            return
                $"{Config.SSOUrl}/{Config.AuthVersion}/oauth/authorize/?response_type=code&redirect_uri={Uri.EscapeDataString(Config.CallbackUrl)}&client_id={Config.ClientId}&scope={string.Join(" ", requestedScopes)}&state={state}{code_challenge}";
        }

        /// <summary>
        ///     Gets a Token from the EVE SSO
        /// </summary>
        public static async Task<Token> GetToken(string grant_type, string code, string codeChallenge = null)
        {
            string body;
            if (grant_type == Token.AUTH)
            {
                if (!string.IsNullOrEmpty(codeChallenge))
                {
                    var bytes = Encoding.ASCII.GetBytes(codeChallenge);
                    var base64 = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
                    body = $"{{\"grant_type\":\"{grant_type}\",\"code\":\"{code}\",\"code_verifier\":\"{base64}\"}}";
                }
                else
                {
                    body = $"{{\"grant_type\":\"{grant_type}\",\"code\":\"{code}\"}}";
                }
            }
            else if (grant_type == Token.REFRESH)
            {
                body = $"{{\"grant_type\":\"{grant_type}\",\"refresh_token\":\"{Uri.EscapeDataString(code)}\"}}";
            }
            else
            {
                throw new Exception("unrecognized auth type: " + grant_type);
            }

            HttpContent postBody = new StringContent(body, Encoding.UTF8, "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Config.ClientKey);

            var responseBase = await Client.PostAsync($"{Config.SSOUrl}/v2/oauth/token", postBody);

            var response = await responseBase.Content.ReadAsStringAsync();

            if (responseBase.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeAnonymousType(response, new {error_description = string.Empty})
                    .error_description;
                throw new ArgumentException(error);
            }

            var token = JsonConvert.DeserializeObject<Token>(response);

            return token;
        }


        /// <summary>
        ///     Verifies the Character information for the provided Token information.
        ///     The feeds the character the token data for storage.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Character</returns>
        public static async Task<Character> Verify(Token token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var response = await Client.GetAsync($"{Config.SSOUrl}/oauth/verify").Result.Content.ReadAsStringAsync();
            var authorizedCharacter = JsonConvert.DeserializeObject<Character>(response);
            //Push Token into character.
            authorizedCharacter.ConsumeToken(token);

            /*
            {"CharacterID":95465499,
            "CharacterName":"CCP Bartender",
            "ExpiresOn":"2017-07-05T14:34:16.5857101",
            "Scopes":"esi-characters.read_standings.v1",
            "TokenType":"Character",
            "CharacterOwnerHash":"lots_of_letters_and_numbers",
            "IntellectualProperty":"EVE"}
            */


            return authorizedCharacter;
        }


        public static async Task<List<Notification>> GetNotificationsByCharacterId(long characterID, string AccessToken)
        {
            var url = $"{Config.EsiUrl}/latest/characters/{characterID}/notifications/?datasource={Config.DataSource}";
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var response = await Client.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ArgumentException("Cannot get notifcaitons:" + await response.Content.ReadAsStringAsync());

            return JsonConvert.DeserializeObject<List<Notification>>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<Structure> GetStructurePublicInfo(int id, string AccessToken)
        {
            var url = $"{Config.EsiUrl}/latest/universe/structures/{id}/?datasource={Config.DataSource}";
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var response = await Client.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ArgumentException("Cannot get Structure:" + await response.Content.ReadAsStringAsync());

            return JsonConvert.DeserializeObject<Structure>(await response.Content.ReadAsStringAsync());
        }
    }
}