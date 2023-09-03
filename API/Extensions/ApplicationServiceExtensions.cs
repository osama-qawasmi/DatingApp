using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddSignalR();
            //services.AddSignalR(e => {e.MaximumReceiveMessageSize = 102400000;});
            services.AddSingleton<PresenceTracker>();

            return services;
        }
    }
}