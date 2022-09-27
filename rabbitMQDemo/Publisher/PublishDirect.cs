using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class PublishDirect
    {

        public void Publish()
        {
            Console.WriteLine("Publish start!");

            var factory = new ConnectionFactory()//初始化
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                VirtualHost = "myhost"
            };

            using var connection = factory.CreateConnection();//创建一个连接

            using var channel = connection.CreateModel();//创建一个信道

            var exchangeName = "exchange_name_direct";
            channel.ExchangeDeclare(
                exchange: exchangeName,//交换机定义一次就可以了 可以在发布端和订阅都做定义重复不会再次创建
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);

            //模拟Direct  可以不需要指定队列 ，在订阅的时候去生成队列名称并绑定到指定的交换机 消息是直接给交换机的
            //channel.QueueDeclare(queue: "hello",//定义一个队列
            //                           durable: false,//消息是否持久化，默认情况下消息会被存到rabbtmq服务器所在的内存中，为了避免rabbtmq宕机消息丢失情况，这里就可以设置true
            //                           exclusive: false,//是否当前连接专属队列
            //                           autoDelete: false,//是否自动删除，当没有生产者或者消费者使用此队列，该队列会自动删除。
            //                           arguments: null);//其它参数，可用于绑定私信队列

            for (int i = 0; i < 1000; i++)
            {
                var message = $"我是消息{i}。";
                if (message == null)
                {
                    break;
                }
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent  //持久化关键参数
                //消息内容
                byte[] body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchangeName,
                               routingKey: "key.like",
                               basicProperties: properties,
                               mandatory:true,
                               body: body);
                Console.WriteLine("成功发送消息:" + message);

                System.Threading.Thread.Sleep(1000);
            }

            Console.ReadLine();

        }
    }
}

