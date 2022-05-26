using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using System;

namespace HelloLinux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();
            try
            {
                logger.Debug("Инициализация программы");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Закрытие программы");
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
