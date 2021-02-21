using Domain;
using Persistence;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace UdpServer
{
    public class Udp
    {

        private const int listenPort = 3030;

        public async static void StartListener(DataContext dataContext)

        {

            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try

            {
                Console.WriteLine($"Waiting for broadcast, {listenPort}");

                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    var governorData = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine(governorData);
                    await SaveDB(governorData, dataContext);
                }

            }

            catch (SocketException e)

            {

                Console.WriteLine(e);

            }
        }

        private async static Task SaveDB(string  mainString , DataContext dataContext)
        {

            if (mainString != null)
            {
                //var stringValuesTest = "16/12/2020," + //0
                         //"15:33:18," +
                         //"868715034074066," +//2
                         //"john wahomeKDB " +
                         //"564R," +//4
                         //"0.000000," +
                         //"0," + //6
                         //"0.399453," +
                         //"S," + //8
                         //"36.962791," +
                         //"E," + //10
                         //"0," +
                         //"1," + //12
                         //"322.799988," +
                         //"1," + //14
                         //"0";
                var values = mainString.Split(',');
                
                var speedGov = dataContext.SpeedGovernors.Where(s => s.Imei == values[2]).FirstOrDefault();

                var location = new Location()
                {
                    Latitude = Double.Parse(values[7]),
                    Long = Double.Parse(values[9]),
                    EngineON = values[11].ToString(),
                    SpeedSignalStatus = values[12].ToString(),
                    Time = values[1].ToString(),
                    GpsCourse = values[13].ToString(),
                    Speed = values[5].ToString(),
                    SpeedGovernor = speedGov
                    // TODO Add some more fields, gps on = 14, ignition = 15, overspeed = 16, odometer = 6, vibration, fuellevel
                };

                dataContext.Locations.Add(location);
                await dataContext.SaveChangesAsync();               
            }
        }
    }
}
