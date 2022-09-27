using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber1
{
    public class SubscriberDirect
    {
        public void Subscriber()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                VirtualHost = "myhost"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var exchangeName = "exchange_name_direct";
                //var queueName = channel.QueueDeclare(durable: true).QueueName;

                var queueName = "queueName_direct1";
                channel.QueueDeclare(queueName
                    , durable: true //持久化核心参数  发布消息时候DeliveryMode =2 这样就可以先启动发布在启动消费端口也不会丢失消息
                    , exclusive: false
                    , autoDelete: false);

                channel.QueueBind(queueName, exchangeName, "color_black");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("接收到信息为 {0}", message);
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.ReadLine();
            }
        }
    }
}
