// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Work Send 启动开始发送消息!");

var factory = new ConnectionFactory()//初始化
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    Port = 5672
};

using (var connection = factory.CreateConnection())//创建一个连接
{
    using (var channel = connection.CreateModel())//创建一个信道
    {
        channel.QueueDeclare(queue: "hello",//定义一个队列
                             durable: false,//消息是否持久化，默认情况下消息会被存到rabbtmq服务器所在的内存中，为了避免rabbtmq宕机消息丢失情况，这里就可以设置true
                             exclusive: false,//是否当前连接专属队列
                             autoDelete: false,//是否自动删除，当没有生产者或者消费者使用此队列，该队列会自动删除。
                             arguments: null);//其它参数，可用于绑定私信队列

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        for (int i = 0; i < 1000; i++)
        {

            var message = $"我是消息{i}。";
            if (message == null)
            {
                break;
            }
            //消息内容
            byte[] body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",//会使用默认的exchange    direct类型
                           routingKey: "hello",
                           basicProperties: properties,
                           body: body);
            Console.WriteLine("成功发送消息:" + message);

            System.Threading.Thread.Sleep(1000);
        }
    }
}

Console.ReadLine();