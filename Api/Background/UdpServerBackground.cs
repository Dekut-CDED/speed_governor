using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Api.SignalRhub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Persistence;
using Serilog;

namespace Api.Background
{
    using Application.Interfaces;
    using Domain;
    public sealed class UdpServerBackground : BackgroundService
    {
        private readonly IHubContext<SignalRealTimeLocation> locationhub;
        private char[] chartoTrim = { 'b', '\'' };
        private UdpClient listner;
        private IPEndPoint groupEp;
        private readonly IConfiguration _config;
        private DataContext _dataContext;
        private readonly IUnitofWork unitofwork;

        public UdpServerBackground(IHubContext<SignalRealTimeLocation> locationhub,
            IConfiguration config, DataContext dataContext, IUnitofWork unitofwork)
        {
            this._config = config;
            this.locationhub = locationhub;
            this._dataContext = dataContext;
            this.unitofwork = unitofwork;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var port = int.Parse(_config["UdpPort"]);
            groupEp = new IPEndPoint(IPAddress.Any, port);
            listner = new UdpClient(groupEp);
            Log.Information("waiting for bloadcast on port {port}");
            while (true)
            {
                var bytes = await listner.ReceiveAsync();
                var data = Encoding.ASCII.GetString(bytes.Buffer, 0, bytes.Buffer.Length);
                //var result = data.TrimStart(chartoTrim).Trim(chartoTrim).Split(",");

                if (data != null)
                {
                    var location = JsonConvert.DeserializeObject<Location>(data);
                    var speedGov = _dataContext.SpeedGovernors.Where(s => s.Imei == location.SpeedGovId).FirstOrDefault();
                    location.SpeedGovernor = speedGov;

                    if (speedGov != null)
                    {

                        // var location = new Location()
                        // {
                        //     Latitude = Double.Parse(result[6]),
                        //     Long = Double.Parse(result[9]),
                        //     Date = result[7],
                        //     EngineON = result[4],
                        //     SpeedSignalStatus = result[5],
                        //     Time = result[8],
                        //     GpsCourse = result[3],
                        //     Speed = result[5].ToString(),
                        //     SpeedGovernor = speedGov
                        //     // TODO Add some more fields, gps on = 14, ignition = 15, overspeed = 16, odometer = 6, vibration, fuellevel
                        // };
                        await locationhub.Clients.All.SendAsync("SpeedGovernorlocation", location);
                        unitofwork.Location.Add(location);
                        unitofwork.Save();
                    }
                }
            }

        }


    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 


}
