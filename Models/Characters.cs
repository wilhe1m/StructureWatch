using System;

namespace wilhe1m.StructureWatch.Models
{
    public class Character
    {
        public int Id { get; set; }
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Scopes { get; set; }
        public string TokenType { get; set; }
        public string CharacterOwnerHash { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }


        public void ConsumeToken(Token token)
        {
            AccessToken = token.AccessToken;
            RefreshToken = token.RefreshToken;
            TokenType = token.TokenType;
            ExpiresAt = token.ExpiresAt;
        }
    }
}