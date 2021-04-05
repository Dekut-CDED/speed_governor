using System;
using System.Threading;
using System.Threading.Tasks;
using Api.SignalRhub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using static Persistence.JsonSerilization;

namespace Api.Background
{
    public sealed class DumyLocationPub : BackgroundService
    {

        string path = @"/media/data/repos/repos/speed_governor_api/Api/Background/points.json";

        private readonly IHubContext<SignalRealTimeLocation> locationbrokerhubcontext;
        public DumyLocationPub(IHubContext<SignalRealTimeLocation> locationbrokerhubcontext)
        {
            this.locationbrokerhubcontext = locationbrokerhubcontext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var json =  System.IO.File.ReadAllText(path);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(json); 
             foreach (var item in myDeserializedClass.features[0].geometry.coordinates)
             {
                 Console.WriteLine(item);
             }

            Console.WriteLine($"Started bloadcasting locations");
            var length = myDeserializedClass.features[0].geometry.coordinates.Count;
            var i = 0;
            while (!stoppingToken.IsCancellationRequested)
             {
                await Task.Delay(100);
                await locationbrokerhubcontext.Clients.All.SendAsync("onMessageReceived", myDeserializedClass.features[0].geometry.coordinates[i], stoppingToken);
                i++;
            }
        }
    }
}