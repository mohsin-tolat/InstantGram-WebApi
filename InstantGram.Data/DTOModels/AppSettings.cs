namespace InstantGram.Data.DTOModels
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public ApplicationDomainConfigurations ApplicationDomainConfiguration { get; set; }

        public UIRenderSettings UIRenderSetting { get; set; }

        public class ApplicationDomainConfigurations
        {
            public string InstantGramApiUrl { get; set; }

            public string InstantGramUIUrl { get; set; }
        }

        public class UIRenderSettings
        {
            public int UserPageSizeForList { get; set; }

            public int PostPageSizeForDashboard { get; set; }

            public int PostPageSizeForUserProfile { get; set; }

            public int PostPageNoForUserProfile { get; set; }
        }
    }
}