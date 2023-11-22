using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Cybtans.Authentication
{
    public class JwtkModel 
    { 

        public string Kty { get; set; }

        public string Use { get; set; }

        public string Kid { get; set; }

        public string E { get; set; }

        public string N { get; set; }

        public string Alg { get; set; }

        public string X5t { get; set; }

        public List<string> X5c { get; set; }       

    }

    public class TokenResult
    {      
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public string TokenType { get; set; }

        public string RefreshToken { get; set; }
     
    }

    public class TokenRequest
    {
      
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        [Required]
        public string? GrandType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public string Hint { get; set; }     
    }


}
