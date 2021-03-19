using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.SignalRhub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using static Persistence.JsonSerilization;
using Newtonsoft.Json;

namespace Api.Background
{
    public sealed class LocationBrokerPub : BackgroundService
    {
        private readonly IHubContext<SignalRealTimeLocation> locationbrokerhubcontext;

        public LocationBrokerPub(IHubContext<SignalRealTimeLocation> locationbrokerhubcontext)
        {
            this.locationbrokerhubcontext = locationbrokerhubcontext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
             
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
                var eventMessage = new EventMessage($"Id {Guid.NewGuid():N}", $"Title {Guid.NewGuid():N}", DateTime.UtcNow);
                await locationbrokerhubcontext.Clients.All.SendAsync("onMessageReceived", eventMessage, stoppingToken);
            }
        }
    }
};