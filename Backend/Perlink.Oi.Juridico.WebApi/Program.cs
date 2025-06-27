using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Perlink.Oi.Juridico.WebApi
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
            //.UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .ConfigureLogging(logging =>
            {
                //Comentei para que as queries aparecesse
                // logging.ClearProviders();
                logging.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Trace);
            });
            //.UseNLog();  // NLog: setup NLog for Dependency injection;
        }
    }
#pragma warning disable CS1591
}
