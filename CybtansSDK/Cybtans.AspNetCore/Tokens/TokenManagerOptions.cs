namespace Cybtans.AspNetCore
{
    public class TokenManagerOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string TokenEndpoint { get; set; }

        public string Scope { get; set; }
    }
}
