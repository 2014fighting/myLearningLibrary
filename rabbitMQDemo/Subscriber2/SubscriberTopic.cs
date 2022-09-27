﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber2
{
    public class SubscriberTopic
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
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var exchangeName = "exchange_name_topic";

            var queueName = "queueName_topic2";
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false);

            channel.QueueBind(queueName, exchangeName, "size.*");

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
