using System.Text;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;
using Api.SignalRhub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Api.Background
{
    public sealed class UdpServerBackground : BackgroundService
    {
        private readonly IHubContext<SignalRealTimeLocation> locationhub;
        private char[] chartoTrim = { 'b', '\'' };
        private UdpClient listner;
        private IPEndPoint groupEp;
        private readonly IConfiguration _config;
        public UdpServerBackground(IHubContext<SignalRealTimeLocation> locationhub, IConfiguration config)
        {
            this._config = config;
            this.locationhub = locationhub;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var port = int.Parse(_config["UdpPort"]);
            groupEp = new IPEndPoint(IPAddress.Any, port);
            listner = new UdpClient(groupEp);

            //redis
            //string recordkey = "Speedgovernor_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
           // speedgovernors = await cache.GetRecordAsync<WeatherForcast[]>(recordkey);

            Console.WriteLine($"waiting for bloadcast at port {port}");
            while (true)
            {
                var bytes = await listner.ReceiveAsync();
                var data = Encoding.ASCII.GetString(bytes.Buffer, 0, bytes.Buffer.Length);
                var result = data.TrimStart(chartoTrim).Trim(chartoTrim).Split(",");
                await locationhub.Clients.All.SendAsync("onMessageReceived", result, stoppingToken);
            }
        }
    }
}