using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(uploaddownloadfiles.Areas.Identity.IdentityHostingStartup))]
namespace uploaddownloadfiles.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}