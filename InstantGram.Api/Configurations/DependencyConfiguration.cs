using InstantGram.Core.Insterface;
using InstantGram.Core.Service;
using Microsoft.Extensions.DependencyInjection;

namespace InstantGram.Api.Configuration
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDependency(this IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
        }
    }
}