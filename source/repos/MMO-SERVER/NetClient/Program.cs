using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
// using Proto;

//socket发送字节数组
static void SendMessage(Socket socket, byte[] body)
{
    byte[] lenBytes = BitConverter.GetBytes(body.Length);
    socket.Send(lenBytes);
    socket.Send(body);
}

//服务器的IP、端口
var host = "127.0.0.1";
int port = 32510;
//服务器终端
IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(iPEndPoint);

Console.WriteLine("成功连接到服务器");

string text = "大河之剑天上来";
// 将字符串转为字节数组
byte[] body = Encoding.UTF8.GetBytes(text);
SendMessage(socket, body);
//SendMessage(socket, Encoding.UTF8.GetBytes("劝君更尽一杯酒"));
//Console.ReadKey();
while (true)
{
    Console.WriteLine("请输入要发送的内容");
    string txt = Console.ReadLine();
    SendMessage(socket, Encoding.UTF8.GetBytes(txt));
}

// Proto.Vector3 vector = new Proto.Vector3();
// vector.X = 7;
// vector.Y = 8;
// vector.Z = 9;

// 用户登录消息
Proto.Package package = new Proto.Package();
package.Request = new Proto.Request();
package.Request.UserLogin = new Proto.UserLoginRequest();
package.Request.UserLogin.Username = "xiazm";
package.Request.UserLogin.Password = "123456";

// package.Response = new Proto.Response();

MemoryStream rawOutput = new MemoryStream();
CodedOutputStream output = new CodedOutputStream(rawOutput);
package.WriteTo(output);
output.Flush();
SendMessage(socket, rawOutput.ToArray());

Console.ReadLine();
