// See https://aka.ms/new-console-template for more information
using Subscriber2;

Console.WriteLine("Subscriber2 开始订阅!");

//new SubscriberFanout().Subscriber();

new SubscriberDirect().Subscriber();