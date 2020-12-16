using Domain;
using Persistence;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
                    await SaveDB(governorData, dataContext);
                }

            }

            catch (SocketException e)

            {

                Console.WriteLine(e);

            }
        }

        private async static Task SaveDB(string stringValues , DataContext dataContext)
        {

            if (stringValues != null)
            {
                var stringValues = "16/12/2020," + //0
                         "15:33:18," +
                         "868715034074066," +//2
                         "john wahomeKDB " +
                         "564R," +//4
                         "0.000000," +
                         "0," + //6
                         "0.399453," +
                         "S," + //8
                         "36.962791," +
                         "E," + //10
                         "0," +
                         "1," + //12
                         "322.799988," +
                         "1," + //14
                         "0";
                var values = stringValues.Split(',');

                var appUser = new AppUser() { UserName = values[3] };

                var speedGovernor = new SpeedGovernor()
                {
                    Owner = appUser,
                    Imei = values[2],
                    PlateNumber = values[4],

                };

                var location = new Location()
                {
                    Latitude = stringValues[7] + stringValues[8],
                    Long = stringValues[9] + stringValues[10],
                    EngineON = stringValues[11].ToString(),
                    SpeedSignalStatus = stringValues[12].ToString(),
                    Time = stringValues[1].ToString(),
                    GpsCourse = stringValues[13].ToString(),
                    Speed = stringValues[5].ToString(),
                    SpeedGovernor = speedGovernor
                    // TODO Add some more fields, gps on = 14, ignition = 15, overspeed = 16, odometer = 6
                };

                var speedGov = await dataContext.SpeedGovernors.FindAsync(speedGovernor);
                if (speedGov == null)
                {
                    dataContext.SpeedGovernors.Add(speedGovernor);
                }

                var currentUser = await dataContext.Users.FindAsync(appUser);
                if (currentUser == null)
                {
                    dataContext.Users.Add(appUser);
                }

                dataContext.Locations.Add(location);
                await dataContext.SaveChangesAsync();
                

            }

        }
    }
}
