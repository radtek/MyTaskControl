using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HongFireSimple
{
    class Program
    {
        public readonly Logger CommonLog = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 连接配置
        /// </summary>
        private static readonly ConnectionFactory rabbitMqFactory = new ConnectionFactory()
        {
            HostName = "192.168.100.205",
            UserName = "Lixf",
            Password = "123456",
            Port = 5672,
            AutomaticRecoveryEnabled = true//断线重连

        };
        /// <summary>
        /// 路由名称
        /// </summary>
        const string ExchangeName = "LiFengFeng.exchange";
        //队列名称
        const string QueueName = "LiFengFeng.queue";
        static void Main(string[] args)
        {
            #region HangFire调用示例
            //GlobalConfiguration.Configuration
            //   .UseNLogLogProvider()
            //   .UseSqlServerStorage("Data Source=localhost;User Id=sa;Password=123456;Database=HangFireSimple;Pooling=true;Max Pool Size=5000;Min Pool Size=0;");

            //Console.WriteLine("Hangfire Server started. Press any key to exit...");
            //var server = new BackgroundJobServer();
            ////支持基于队列的任务处理：任务执行不是同步的，而是放到一个持久化队列中，以便马上把请求控制权返回给调用者。
            //var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("{0}===》这是队列任务!", DateTime.Now.ToString("HH:mm:ss")));

            ////延迟任务执行：不是马上调用方法，而是设定一个未来时间点再来执行。
            //BackgroundJob.Schedule(() => Console.WriteLine("{0}===》这是延时任务!", DateTime.Now.ToString("HH:mm:ss")), TimeSpan.FromSeconds(5));

            ////循环任务执行：一行代码添加重复执行的任务，其内置了常见的时间循环模式，也可基于CRON表达式来设定复杂的模式。
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("{0}===》这是每分钟执行的任务!", DateTime.Now.ToString("HH:mm:ss")), Cron.Minutely); //注意最小单位是分钟

            ////延续性任务执行：类似于.NET中的Task,可以在第一个任务执行完之后紧接着再次执行另外的任务
            //BackgroundJob.ContinueWith(jobId, () => Console.WriteLine("{0}===》这是延续性任务!", DateTime.Now.ToString("HH:mm:ss")));
            #endregion


            Console.WriteLine("生产者发送RabbitMQ消息...");
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeName, "direct", durable: true, autoDelete: false, arguments: null);
                    channel.QueueDeclare(QueueName, durable: true, autoDelete: false, exclusive: false, arguments: null);
                    channel.QueueBind(QueueName, ExchangeName, routingKey: QueueName);

                    var props = channel.CreateBasicProperties();
                    //MQ消息持久化
                    props.Persistent = true;
                    for (int i = 0; i < 10; i++)
                    {
                        string content = $"Rabbit消息--{i + 1}";
                        var msgBody = Encoding.UTF8.GetBytes(content);
                        channel.BasicPublish(exchange: ExchangeName, routingKey: QueueName, basicProperties: props, body: msgBody);
                        Console.WriteLine($"***发送时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}，{content}发送完成!");
                    }
                }
            }
            Console.WriteLine("RabbitMQ消息已发送完成...");

            var factory = new ConnectionFactory() { HostName = "192.168.100.205", UserName = "Lixf", Password = "123456", VirtualHost = "/",Port = 5672,AutomaticRecoveryEnabled = true };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "LiFengFeng.queue",
                                     durable: true,//可持久化到内存
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("******* 接收到消息 {0}", message);
                    //确认已消费/接收消息
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(queue: "LiFengFeng.queue",
                                     autoAck: true,"Tags",
                                     consumer: consumer);

                Console.WriteLine("  [enter] to exit.");
                Console.ReadLine();
            }
            Console.ReadKey();


        }
    }
}
