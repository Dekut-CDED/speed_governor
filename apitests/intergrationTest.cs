using System.Net.Http;
using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace apitests
{
    public class IntergrationTest
    {
        protected readonly HttpClient TestClient;
        [Fact]
        public void IntergrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration(service =>
                {
                    service.RemoveAll(typeof(DataContext))
                   service.AddDbContext<DataContext>(options =>
                   {
                       options.UseInMemoryDatabase("database")
                           })

                }) });
            TestClient = appFactory.CreateClient();
        }

        protected async Task Authenticate()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync())
        }

        private async Task<string> GetJwtAsync()
        {



        }
    }
}
