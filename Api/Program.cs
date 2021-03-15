using System;
using System.IO;
using System.Threading;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Serilog;
using UdpServer;

namespace Api
{
    public class Program
    {
      
     public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        public static void Main(string[] args)
        {
            
              Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger()
                ;

            var host = CreateHostBuilder(args).Build();
            var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                Log.Information("Getting the Speed Governors Engines Up");
                var usermanager = services.GetRequiredService<UserManager<AppUser>>();
                var context = services.GetRequiredService<DataContext>();

                Log.Information("Migrating the database from the migrations");
                context.Database.Migrate();

                Thread t = new Thread(new ThreadStart(()=> {
                        Udp.StartListener(context);
                                }));
                t.Start();                   
             host.Run();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurrred during Migrations");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
