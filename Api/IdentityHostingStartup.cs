using System;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

[assembly: HostingStartup(typeof(UploadandDowloadService.Data.IdentityHostingStartup))]
namespace UploadandDowloadService.Data
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {

                services.AddDbContext<DataContext>(opt =>
               {
                   opt.UseSqlServer("Server=tcp:kaizenserver.database.windows.net,1433;Initial Catalog=cded;Persist Security Info=False;User ID=kaizenadmin;Password=pK}w/5cfF<RwvRjWGSB5RJ=^^;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
               });

                var builder = services.AddIdentityCore<AppUser>();
                var identitybuilder = new IdentityBuilder(builder.UserType, builder.Services);
                identitybuilder.AddRoles<IdentityRole>();
                identitybuilder.AddEntityFrameworkStores<DataContext>();
                identitybuilder.AddSignInManager<SignInManager<AppUser>>();

            });
        }
    }
}