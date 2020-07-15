using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MordorFanficWeb.BusinessLogic.Helpers;
using MordorFanficWeb.BusinessLogic.Interfaces;
using MordorFanficWeb.Models;
using NLog.Web;

namespace MordorFanficWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var accountService = services.GetRequiredService<IAccountService>();
                        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        await RolesInitializer.InitializeAsync(accountService, rolesManager).ConfigureAwait(false);
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex, "Error initializing admin role");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
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
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseNLog();
    }
}
