using InstantGram.Core.Insterface;
using InstantGram.Core.Service;
using InstantGram.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InstantGram.Api.Configuration
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDependency(this IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IUserService, UserService>();

            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("ApplicationDbContext"));
        }
    }
}