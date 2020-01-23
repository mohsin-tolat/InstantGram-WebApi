using InstantGram.Common.Domain.Interface;
using InstantGram.Common.Domain.Service;
using InstantGram.Core.Insterface;
using InstantGram.Core.Service;
using InstantGram.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstantGram.Api.Configuration
{
    public static class DependencyConfiguration
    {
        public static void ConfigureDependency(this IServiceCollection services, IConfiguration databaseConfiguration)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUserResolverService, UserResolverService>();


            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(databaseConfiguration.GetConnectionString("InstantGramDbContext")));
        }
    }
}