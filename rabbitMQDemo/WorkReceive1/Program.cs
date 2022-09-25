// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Work Receive1 启动，开始接收消息");


var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    Port = 5672,
    VirtualHost= "myhost"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: "hello",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

//均衡调度核心设置=》预处理消息数量设定，标识在消费者没有确认消息之前不会再这个消费者发送新的消息
//如果没有该设置默认是循环调度也就是不管消费者处理能力平均分配消息
//注释改行代码分别启动两个消费者和发送者就能看出区别
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    System.Threading.Thread.Sleep(5000);//模拟消费者执行效率
    Console.WriteLine("1接收到信息为 {0}", message);
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};
channel.BasicConsume(queue: "hello",
                     autoAck: false,
                     consumer: consumer);

Console.ReadLine();

