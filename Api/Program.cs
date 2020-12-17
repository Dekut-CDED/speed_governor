using System;
using System.Threading;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using UdpServer;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

             var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                try
                {
                    var usermanager = services.GetRequiredService<UserManager<AppUser>>();
                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                    SeedData.SeedActivities(context, usermanager).Wait();


                    Thread t = new Thread(new ThreadStart(()=> {
                        Udp.StartListener(context);
                                }));
                    t.Start();                   
                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during Migrations");
                }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
