using System;



namespace wilhe1m.StructureWatch.Models{

    public class Character{
        public int Id{get;set;}
       public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Scopes { get; set; }
        public string TokenType { get; set; }
        public string CharacterOwnerHash { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }


        public void ConsumeToken(Token token){
            this.AccessToken = token.AccessToken;
            this.RefreshToken = token.RefreshToken;
            this.TokenType = token.TokenType; 
            this.ExpiresAt = token.ExpiresAt;
        }
        
    }
}
