// See https://aka.ms/new-console-template for more information
using Subscriber1;

Console.WriteLine("Subscriber1 开始订阅!");

//new SubscriberFanout().Subscriber();


new SubscriberDirect().Subscriber();