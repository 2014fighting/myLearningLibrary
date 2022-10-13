using System.Net.WebSockets;
using System.Text;

namespace WebSocketServer
{
    public class WsTest
    {
        //当前请求实例
        WebSocket? socket = null;
        public async Task DoWork(HttpContext ctx)
        {
            socket = await ctx.WebSockets.AcceptWebSocketAsync();
            //执行监听
            await EchoLoop();
        }
        public async Task EchoLoop()
        {
            //创建缓存区
            var buffer = new byte[1024];
            var seg = new ArraySegment<byte>(buffer);
            while (socket?.State == WebSocketState.Open)
            {
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);
                //判断类型读取
                if (incoming.MessageType == WebSocketMessageType.Text)
                {
                    //incoming.Count  代表，请求内容字节数量
                    string userMessage = Encoding.UTF8.GetString(seg.Array, 0, incoming.Count);
                    //接收客户的字符串
                    userMessage = "You sent: " + userMessage + " at " +
                        DateTime.Now.ToLongTimeString();
                    ArraySegment<byte> segResult = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMessage));
                    await socket.SendAsync(segResult, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                }

                byte[] backInfo = System.Text.UTF8Encoding.Default.GetBytes("服务端相应内容结束");
                var outgoing = new ArraySegment<byte>(backInfo, 0, backInfo.Length);
                await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    public class WebSocketOptions
    {
        TimeSpan KeepAliveInterval { get; set; }  // 发送pingpong 控制帧的时间间隔，默认2min
        IList<string> AllowedOrigins { get; }    // 默认允许所有client origin 跨CORS访问websocket； 如果要配置： 实例化options. options.AllowedOrigins.Add("pingpong.com")
    }
}
