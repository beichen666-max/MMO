using GameServer.Network;  // 使用Game-Sever下的Network
using System.Net;
using System.Net.Sockets;  // ?
using System.Text;

namespace Game_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建监听者对象
            NetService netService = new NetService();
            netService.Init(32510);
            netService.Start();
        }
    }
}