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
using Persistence;
using System.Linq;
using Domain;
using Newtonsoft.Json;

namespace Api.Background
{
    public sealed class UdpServerBackground : BackgroundService
    {
        private readonly IHubContext<SignalRealTimeLocation> locationhub;
        private char[] chartoTrim = { 'b', '\'' };
        private UdpClient listner;
        private IPEndPoint groupEp;
        private readonly IConfiguration _config;
        private DataContext _dataContext;

        public UdpServerBackground(IHubContext<SignalRealTimeLocation> locationhub,
            IConfiguration config, DataContext dataContext)
        {
            this._config = config;
            this.locationhub = locationhub;
            this._dataContext = dataContext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var port = int.Parse(_config["UdpPort"]);
            groupEp = new IPEndPoint(IPAddress.Any, port);
            listner = new UdpClient(groupEp);

            Console.WriteLine($"waiting for bloadcast on port {port}");
            while (true)
            {
                var bytes = await listner.ReceiveAsync();
                var data = Encoding.ASCII.GetString(bytes.Buffer, 0, bytes.Buffer.Length);
                var result = data.TrimStart(chartoTrim).Trim(chartoTrim).Split(",");
                Console.WriteLine(result);

                if (result != null)
                {
                   // Check speedgovernor first
                   //TODO   = Use IMEI as ID or Redis Caching
                    var speedGov = _dataContext.SpeedGovernors.Where(s => s.Imei ==  result[2]).FirstOrDefault();
                    if (speedGov != null)
                    {
                       
                        var location = new Location()
                        {
                            Latitude = Double.Parse(result[6]),
                            Long = Double.Parse(result[9]),
                            Date = result[7],
                            EngineON = result[4],
                            SpeedSignalStatus = result[5],
                            Time = result[8],
                            GpsCourse = result[3],
                            Speed = result[5].ToString(),
                            SpeedGovernor = speedGov
                            // TODO Add some more fields, gps on = 14, ignition = 15, overspeed = 16, odometer = 6, vibration, fuellevel
                        };
                        var jsonLocation = JsonConvert.SerializeObject(location);
                        await locationhub.Clients.All.SendAsync(speedGov.Phone, location, stoppingToken);

                        await SaveDB(location);
                    }                   
                }
            }

            async Task SaveDB(Location location)
            {         
                _dataContext.Locations.Add(location);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
