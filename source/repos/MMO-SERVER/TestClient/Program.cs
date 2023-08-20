using System.Net;
using System.Net.Sockets;

namespace TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var host = "127.0.0.1";
            int port = 32510;

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            socket.Connect(iPEndPoint);

            Console.WriteLine("成功连接到服务器");
        }
    }
}