using HelloLinux.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using System;
using System.Threading.Tasks;

namespace HelloLinux
{
    public class Program
    {
        public static async Task  Main(string[] args)
        {
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();
            try
            {
                logger.Debug("Инициализация программы");
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var userManager = services.GetRequiredService<UserManager<User>>();
                        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        await RoleInitializer.InitializeAsync(userManager, rolesManager);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Произошла ошибка при заполнении базы данных.");
                    }
                }
                host.Run();
            
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Аварийное закрытие программы");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
              
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)              
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    // .UseUrls("http://localhost:5201", "http://localhost:5202")
                    .UseStartup<Startup>()
                    .UseNLog();
                });
    }
}
