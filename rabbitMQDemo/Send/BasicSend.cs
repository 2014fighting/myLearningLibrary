using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Send
{
    /// <summary>
    /// 基础工作模式_发送
    /// </summary>
    public class BasicSend
    {
        /// <summary>
        /// 使用基础的方式不指定交换机 使用默认的交换机
        /// </summary>
        public void Send1()
        {
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
                    //while (true)
                    //{
                    //    Console.WriteLine("请输入发送内容并回车:");
                    //    var message = Console.ReadLine();
                    //    if(message == null)
                    //    {
                    //        break;
                    //    }
                    //    //消息内容
                    //    byte[] body = Encoding.UTF8.GetBytes(message);
                    //    channel.BasicPublish(exchange: "",
                    //                   routingKey: "hello",
                    //                   basicProperties: null,
                    //                   body: body);

                    //    Console.WriteLine("成功发送消息:" + message);
                    //}

                }
            }

            Console.ReadLine();
        }

        /// <summary>
        /// 定义并指定一个自己的交换机 ，消费端需要做绑定操作才能正常消费
        /// </summary>
        public void Send2()
        {
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
                    var exchangeName = "exchange_name_test";
                    channel.ExchangeDeclare(
                        exchange: exchangeName,
                        type: "direct",
                        durable: false,
                        autoDelete: false,
                        arguments: null);

                    channel.QueueDeclare(queue: "hello",//定义一个队列
                                         durable: false,//消息是否持久化，默认情况下消息会被存到rabbtmq服务器所在的内存中，为了避免rabbtmq宕机消息丢失情况，这里就可以设置true
                                         exclusive: false,//是否当前连接专属队列
                                         autoDelete: false,//是否自动删除，当没有生产者或者消费者使用此队列，该队列会自动删除。
                                         arguments: null);//其它参数，可用于绑定私信队列


                    for (int i = 0; i < 1000; i++)
                    {

                        var message = $"我是消息{i}。";
                        if (message == null)
                        {
                            break;
                        }
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: exchangeName,
                                       routingKey: "hello",
                                       basicProperties: null,
                                       body: body);
                        Console.WriteLine("成功发送消息:" + message);

                        System.Threading.Thread.Sleep(1000);
                    }
                    //while (true)
                    //{
                    //    Console.WriteLine("请输入发送内容并回车:");
                    //    var message = Console.ReadLine();
                    //    if(message == null)
                    //    {
                    //        break;
                    //    }
                    //    //消息内容
                    //    byte[] body = Encoding.UTF8.GetBytes(message);
                    //    channel.BasicPublish(exchange: "",
                    //                   routingKey: "hello",
                    //                   basicProperties: null,
                    //                   body: body);

                    //    Console.WriteLine("成功发送消息:" + message);
                    //}

                }
            }

            Console.ReadLine();
        }


    }
}
