using Goblin.Application;
using Goblin.Application.Abstractions;
using Goblin.Application.KeyboardCommands;
using Goblin.Application.MergedCommands;
using Goblin.Application.TextCommands;
using Goblin.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace Goblin.WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVkApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IVkApi, VkApi>(x =>
            {
                var token = configuration["Vk:AccessToken"];
                var api = new VkApi();
                api.Authorize(new ApiAuthParams
                {
                    AccessToken = token
                });
                return api;
            });
        }

        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BotDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                       .UseLazyLoadingProxies();
            });
            services.AddDbContext<IdentityUsersDbContext>(options => options.UseSqlServer(connectionString));
        }

        public static void AddBotFeatures(this IServiceCollection services)
        {
            services.AddScoped<ITextCommand, DebugCommand>();
            services.AddScoped<ITextCommand, SetDataCommand>();

            services.AddScoped<IKeyboardCommand, StartCommand>();
            services.AddScoped<IKeyboardCommand, ScheduleKeyboardCommand>();
            services.AddScoped<IKeyboardCommand, WeatherKeyboardCommand>();
            services.AddScoped<IKeyboardCommand, WeatherNowCommand>();
            services.AddScoped<IKeyboardCommand, WeatherDailyCommand>();
            services.AddScoped<IKeyboardCommand, ScheduleCommand>();
            
            services.AddScoped<IKeyboardCommand, HelpCommand>();
            services.AddScoped<ITextCommand, HelpCommand>();
            services.AddScoped<IKeyboardCommand, ExamsCommand>();
            services.AddScoped<ITextCommand, ExamsCommand>();
            services.AddScoped<IKeyboardCommand, GetRemindsCommand>();
            services.AddScoped<ITextCommand, GetRemindsCommand>();
            
            services.AddScoped<CommandsService>();
            services.AddScoped<CallbackHandler>();
        }
    }
}