using System.Net.Sockets;
using System.Net;

namespace Telemetry_Simulator.Classes
{
    public static class Producer
    {
        public static void SendEncodedData(byte[] dataBytes) {
            using (var udpClient = new UdpClient())
            {
                IPEndPoint endPoint = new(IPAddress.Parse("127.0.0.1"), 8000);
                udpClient.Send(dataBytes, dataBytes.Length, endPoint);
                udpClient.Close();
            }
        }
    }
}
