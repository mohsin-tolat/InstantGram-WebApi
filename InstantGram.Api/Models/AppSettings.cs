namespace InstantGram.Api.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public ApplicationDomainConfiguration ApplicationDomainConfig { get; set; }

        public class ApplicationDomainConfiguration
        {
            public string InstantGramApiUrl { get; set; }

            public string InstantGramUIUrl { get; set; }
        }
    }
}