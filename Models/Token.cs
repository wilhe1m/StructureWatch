using System;
using Newtonsoft.Json;

namespace wilhe1m.StructureWatch.Models{
    public class Token{
        public int Id{get;set;}

        public static readonly string REFRESH= "refresh_token";
        public static readonly string AUTH = "authorization_code";
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime Created{get; set;} = DateTime.UtcNow;

        public DateTime ExpiresAt{get{return this.Created.AddSeconds(this.ExpiresIn);}}
    }
}