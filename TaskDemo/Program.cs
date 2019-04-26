using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Threading;
using LiFFHelper.LogManager;

namespace TaskDemo
{
    class Program
    {
        static LogUitility log = new LogUitility(AppContext.BaseDirectory + @"\Log");
        static async Task AsyncLambda()
        {
            Func<string, Task<string>> lambdaTask = async name =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return $"{name} Task is Runing Thread is  ThreadPool:{Thread.CurrentThread.IsThreadPoolThread}" + $"ThreadPool worker Threadid is  :{Thread.CurrentThread.ManagedThreadId}";
            };
            string result = await lambdaTask("Async Lambda Task");
            WriteLine(result);
        }
        static async Task<string> GetInfoaSyncQuickly(string Name, int num)
        {
            int maxWork, queuewrork;
            ThreadPool.GetMaxThreads(out maxWork, out queuewrork);
            WriteLine($"Task {Name} is started !");
            //Task.Delay实现内部实现计时器，并将后面的代码放入计时器之后。
            //当前工作线程会被立即放回到线程池中，等待计时器时间到达触发计时器事件，再会从线程池中拿出一个工作者线程
            //当前的这个Task对象就相当于一个工作线程
            await Task.Delay(TimeSpan.FromSeconds(num));
            for (int i = 0; i < 50; i++)
            {
                log.Debug($"Can Work...");
            }
            //await Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(num)); });
            return $"{Name} Task is Runing Thread is  ThreadPool:{Thread.CurrentThread.IsThreadPoolThread}" + $"ThreadPool worker Threadid is  :{Thread.CurrentThread.ManagedThreadId}  线程池最大工作线程数：{maxWork} 当前等待中：{queuewrork}";
        }
        static async Task<string> GetInfoaSyncLow(string Name, int num)
        {
            WriteLine($"Task {Name} is started !");
            //await Task.Delay(TimeSpan.FromSeconds(num));
            await Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(num)); });
            for (int i = 0; i < 50; i++)
            {
                log.Info($"Can Work...");
            }
            return $"{Name} Task is Runing Thread is  ThreadPool:{Thread.CurrentThread.IsThreadPoolThread}" + $"ThreadPool worker Threadid is  :{Thread.CurrentThread.ManagedThreadId}";
        }
        static async Task MyAsyncWithAwaitQuickly()
        {
            Task<string> t1 = GetInfoaSyncQuickly("A", 2);
            Task<string> t2 = GetInfoaSyncQuickly("B", 4);
            Task<string> t3 = GetInfoaSyncQuickly("C", 6);
            Task<string> t4 = GetInfoaSyncQuickly("D", 8);
            Task<string> t5 = GetInfoaSyncQuickly("E", 10);
            Task<string> t6 = GetInfoaSyncQuickly("F", 12);
            Task<string> t7 = GetInfoaSyncQuickly("G", 14);
            Task<string> t8 = GetInfoaSyncQuickly("H", 16);
            string[] rs = await Task.WhenAll(t1, t2,t3,t4,t5,t6,t8);
            foreach (var item in rs)
            {
                WriteLine(item);
            }
        }
        static async Task MyAsyncWithAwaitLow()
        {
            Task<string> t1 = GetInfoaSyncLow("DemoA", 5);
            Task<string> t2 = GetInfoaSyncLow("Boom", 10);
            string[] rs = await Task.WhenAll(t1, t2);
            foreach (var item in rs)
            {
                WriteLine(item);
            }
        }
        static void Main(string[] args)
        {
            Task tq = MyAsyncWithAwaitQuickly();
            tq.Wait();
            //Task tl = MyAsyncWithAwaitLow();
            //tl.Wait();

            WriteLine("------------------------------");
            ReadLine();
            Task t = AsyncLambda();
            t.Wait();


        }
    }
}
