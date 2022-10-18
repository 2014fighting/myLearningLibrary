// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
var buffer = new byte[1024 * 1024 * 2];
string ip = "127.0.0.1";
int port = 1111;
Console.WriteLine("客户端启动.....");
Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);
client.Connect(point);
Console.WriteLine("连接服务器成功");
try
{
  
    int length = client.Receive(buffer);
    Console.WriteLine("接收服务器{0},消息:{1}", client.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(buffer, 0, length));
    // 像服务器发送消息
    for (int i = 0; i < 10; i++)
    {
        Thread.Sleep(2000);
        string sendMessage = string.Format("客户端发送的消息,当前时间{0}", DateTime.Now.ToString());
        client.Send(Encoding.UTF8.GetBytes(sendMessage));
        Console.WriteLine("向服务发送的消息:{0}", sendMessage);
    }
}
catch (Exception ex)
{
    client.Shutdown(SocketShutdown.Both);
    client.Close();
    Console.WriteLine(ex.Message);
}
Console.WriteLine("发送消息结束");
Console.ReadKey();
