using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace Launchwares.Core
{
    public static class AksunucumUpd
    {
        public static Timer UPDTimer = new Timer() {
            Interval = 10000,
            Enabled = false
        };

        public static AksunucumSettings Settings = new AksunucumSettings();

        public async static void Start()
        {
            var result = await API.client.CustomGet<dynamic>("https://launchwares.com/storage/files/aksunucum/settings.json");
            Settings = JsonConvert.DeserializeObject<AksunucumSettings>(Convert.ToString(result.settings));
            UPDTimer.Elapsed += UPDTimer_Elapsed;
            UPDTimer.Start();
        }

        public static void UPDTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress serverAddr = IPAddress.Parse(Settings.IP);
            IPEndPoint endPoint = new IPEndPoint(serverAddr, Settings.Port);

            byte[] send_buffer = Encoding.ASCII.GetBytes("aksunucum");
            sock.SendTo(send_buffer, endPoint);
        }
    }

    [JsonObject]
    public class AksunucumSettings
    {
        [JsonProperty("ip")]
        public string IP { get; set; }
        [JsonProperty("port")]
        public int Port { get; set; }
    }
}
