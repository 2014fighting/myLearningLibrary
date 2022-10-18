// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
 var buffer = new byte[1024 * 1024 * 2];
string ip = "127.0.0.1";
int port = 1111;

Console.WriteLine("服务器启动.....");
Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
EndPoint point = new IPEndPoint(IPAddress.Parse(ip), port);

server.Bind(point);//用于绑定 IPEndPoint 对象，在服务端使用。

server.Listen(100);//开始监听



Console.WriteLine("a client connect.....");

Thread thread = new Thread(ListenClientConnect);
thread.Start();
//while (true)
//{
//    string? str = Console.ReadLine();
//    socket.Send(Encoding.UTF8.GetBytes(str));
//}

 void ListenClientConnect()
{
    try
    {
        while (true)
        {
            Socket socket = server.Accept(); //为新建连接创建新的socket
            socket.Send(Encoding.UTF8.GetBytes("服务端发送消息:111"));
            Thread thread = new Thread(ReceiveMessage);
            thread.Start(socket);
        }
    }
    catch (Exception)
    {
    }
}

/// <summary>
/// 接收客户端消息
/// </summary>
/// <param name="socket">来自客户端的socket</param>
 void ReceiveMessage(object socket)
{
    Socket clientSocket = (Socket)socket;
    while (true)
    {
        try
        {
            //获取从客户端发来的数据
            int length = clientSocket.Receive(buffer);
            Console.WriteLine("接收客户端{0},消息{1}", clientSocket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(buffer, 0, length));
            clientSocket.Send(Encoding.UTF8.GetBytes("服务端发送消息:222"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            break;
        }
    }
}
  