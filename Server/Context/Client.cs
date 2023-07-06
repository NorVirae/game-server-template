using System.Net;
using System.Text;
using Server.Core;
using QNetLib;


namespace Server
{
    public class Client
    {
        public NetworkManager manager;
        public int networkID;
        public ClientStatus clientStatus;
        public INetworkClient clientContext;
        
        public bool isConnected;

        public double lastPongTime;
        public double lastPingTime;
        private Datagram pingData;
        private StringBuilder builder;

        public string token;
        public string userID;

        public string sessionId;

        public EndPoint ClientEndpoint => clientContext.ClientEndpoint;

        
        public Client(INetworkClient client, NetworkManager manager)
        {
            clientContext = client;
            this.manager = manager;
            pingData = new Datagram(EventType.Ping, null);
            builder = new StringBuilder();
            lastPongTime = Core.Timer.TotalsecondsSinceStart;
        }

        public void SendPing()
        {
            lastPingTime =  Core.Timer.Now.ToTimeStamp();
            pingData.body = Core.Timer.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’");
            SendDataGram(pingData);
            //Console.WriteLine("sent");
        }

        public void SendDataGram(Datagram datagram)
        {
            datagram.key = userID;
            string data = datagram.ToString();
            byte[] payload = Encoding.ASCII.GetBytes(data);
            clientContext.Send(payload, payload.Length, false, DataFormat.TEXT);
        }

        public void Disconnect()
        {
            clientContext.Close();
        }

        public string BuildTimeData()
        {
            DateTime now = Core.Timer.Now;
            builder.Append("{\"hour\":");
            builder.Append(now.Hour);
            builder.Append(",\"min\":");
            builder.Append(now.Minute);
            builder.Append(",\"sec\":");
            builder.Append(now.Second);
            builder.Append("}");
            return builder.ToString();
        }
    }
}
