using GameServer.Network;  // 使用Game-Sever下的Network
using Network;
using System.Net;
using System.Net.Sockets;  // ?
using System.Text;

namespace Game_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // 创建监听者对象
            TcpSocketListener listener = new TcpSocketListener("0.0.0.0", 32510);
            listener.SocketConnected += OnClientConnected;
            listener.Start();
            Console.ReadKey();

        }

        private static void OnClientConnected(object? sender, Socket socket)
        {
            // 接收到客户端
            var ipEndPoint = socket.RemoteEndPoint as IPEndPoint; // 向下转型，类型还原
            Console.WriteLine("有客户端连入，对方端口是：" + ipEndPoint.Port);

            var lfd = new LengthFieldDecoder(socket, 64 * 1024, 0, 4, 0, 4);
            lfd.DataReceived += OnDataReceived;
            lfd.Start(); // 启动解码器
        }

        private static void OnDataReceived(byte[] buffer)
        {
            string txt = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(txt);
        }
    }
}