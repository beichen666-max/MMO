using System.Net.Sockets;
using System.Net;
using System.Text;

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
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(iPEndPoint);

            Console.WriteLine("成功连接到服务器");

            string text = "大河之剑天上来";
            // 将字符串转为字节数组
            byte[] body = Encoding.UTF8.GetBytes(text);
            SendMessage(socket,body);
            SendMessage(socket, Encoding.UTF8.GetBytes("劝君更尽一杯酒"));
            Console.ReadKey ();
        }

        //socket发送字节数组
        static void SendMessage(Socket socket, byte[] body)
        {
            byte[] lenBytes = BitConverter.GetBytes(body.Length);
            socket.Send(lenBytes);
            socket.Send(body);
        }
    }
}