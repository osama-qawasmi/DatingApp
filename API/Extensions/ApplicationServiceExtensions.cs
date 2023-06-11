using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt => 
            {
                string conString = config.GetConnectionString("DefaultConnection");
                opt.UseSqlite(conString);
            });
            services.AddCors();
            services.AddScoped<ITokenService, Services.TokenService>();
            return services;
        }
    }
}