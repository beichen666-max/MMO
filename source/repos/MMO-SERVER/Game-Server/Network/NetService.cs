using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network
{
    // 网络服务
    public class NetService
    {
        // 网络监听器
        TcpSocketListener listener = null;

        public NetService () { }
        public void Init (int port){
            listener = new TcpSocketListener("0.0.0.0", port);
            listener.SocketConnected += OnClientConnected;
        }
        
        public void Start(){
            listener.Start();
        }

        private static void OnClientConnected(object? sender, Socket socket)
        {
            new NetConnection(socket,
                new NetConnection.DataReceivedCallback(OnDataReceived),
                new NetConnection.DisconnectedCallback(OnDisconnected));
        }
        
        private static void OnDisconnected(NetConnection sender)
        {
            Console.WriteLine("连接断开");
        }

        private static void OnDataReceived(NetConnection sender, byte[] data)
        {
            Proto.Vector3 v = Proto.Vector3.Parser.ParseFrom(data);
            Console.WriteLine("x=" + v.X + ",y=" + v.Y + ",z=" + v.Z);
        }
    }
}
