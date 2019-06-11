using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Threading;
using LiFFHelper.LogManager;
using static System.Threading.Thread;
using System.Diagnostics;
//Rx库
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Collections.Concurrent;
using System.Globalization;
//0610使用TPL数据流库来实现并行管道
using System.Threading.Tasks.Dataflow;
using LiFFHelper.LogManager;
using System.Reactive.Subjects;
using System.Reactive.Disposables;

namespace TaskDemo
{
    class Program
    {
        static LogUitility log = new LogUitility(AppContext.BaseDirectory + @"\Log");

        #region 0610使用TPL数据流库来实现并行管道
        static async Task ProcessAsyncChronous()
        {
            var cts = new CancellationTokenSource();
            Random _rnd = new Random(DateTime.Now.Millisecond);
            //创建取消监听任务
            await Task.Run(() => {
                if (ReadKey().KeyChar == 'C')
                {
                    cts.Cancel();
                }
            },cts.Token);

            var inputBlock = new BufferBlock<int>(new DataflowBlockOptions
            {
                BoundedCapacity = 5,
                CancellationToken = cts.Token
            });

            //使用指定的Func委托函数和Tasks.Dataflow.ExecutionDataflowBlockOptions(操作配置)
            var convertToDecimalBlock = new TransformBlock<int, decimal>(n =>
            {
                decimal result = Convert.ToDecimal(n * 100);
                WriteLine($"Decimal converter sent {result} to the next stage on" + $"Thread id {CurrentThread.ManagedThreadId}");
                Sleep(TimeSpan.FromMilliseconds(_rnd.Next(200)));
                return result;
            },new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 4,CancellationToken = cts.Token});
            //使用指定的Func委托函数和Tasks.Dataflow.ExecutionDataflowBlockOptions(操作配置)
            var stringifyBlock = new TransformBlock<decimal, string>(n => {
                string result = $"--{n.ToString("C",CultureInfo.GetCultureInfo("en-us"))}";
                WriteLine($"String Formatter sent {result} to next stage on thread id {CurrentThread.ManagedThreadId}");
                Sleep(TimeSpan.FromMilliseconds(_rnd.Next(200)));
                return result;
            },new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 4,CancellationToken = cts.Token});
            //使用指定的Func委托函数和Tasks.Dataflow.ExecutionDataflowBlockOptions(操作配置)
            var outputBlock = new ActionBlock<string>(s => {
                WriteLine($"The final result is {s} on thread id {CurrentThread.ManagedThreadId}");
            },new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 4,CancellationToken = cts.Token});
            //将源数据连接传递到指定的TransformBlock
            //可传递指定的DataflowLinkOptions配置参数来规定连接属性
            inputBlock.LinkTo(convertToDecimalBlock, new DataflowLinkOptions() { PropagateCompletion = true });
            convertToDecimalBlock.LinkTo(stringifyBlock, new DataflowLinkOptions { PropagateCompletion = true });
            stringifyBlock.LinkTo(outputBlock, new DataflowLinkOptions { PropagateCompletion = true });

            try
            {
                Parallel.For(0, 20, new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = cts.Token },
                    i => {
                        WriteLine($"Added {i} tot source data on thread id {CurrentThread.ManagedThreadId}");
                        //异步向目标消息块/元数据/数据块提供消息，允许延迟。
                        inputBlock.SendAsync(i).GetAwaiter().GetResult();
                    });
                //向DataFlow.IDataFlowBlock发出信号，表明它应该不接受、不生成更多的消息，也不使用更多的延迟消息。
                inputBlock.Complete();
                ////获取表示异步操作的System.Threading.Tasks.Task对象
                //并完成数据流块。
                //等待数据流块完成
                await outputBlock.Completion;
                WriteLine("Press Enter to exit");
            }
            catch (OperationCanceledException)
            {
                WriteLine("Operation has been cnceled! Press Enter To exit.");
            }
        }
        #endregion

        #region 使用BlockingCollection实现并行管道
        private const int CollectionNumber = 4;
        private const int Count = 5;
        static void CreateInitialValues(BlockingCollection<int>[] sourceArrays, CancellationTokenSource cts)
        {
            Parallel.For(0, sourceArrays.Length * Count, (j, state) => {
                if (cts.Token.IsCancellationRequested)
                {
                    state.Stop();
                }
                int number = GetRandomNumber(j);
                //尝试将指定的项添加到任一指定的BlockingCollection实例。
                //向其添加项的集合在 collections 数组中的索引；如果未能添加项，则为 -1。
                int k = BlockingCollection<int>.TryAddToAny(sourceArrays, j);
                //添加成功
                if (k >= 0)
                {
                    WriteLine($"Add{j} to source data on thread" + $"id:{CurrentThread.ManagedThreadId}");
                    Sleep(TimeSpan.FromMilliseconds(number));
                }
            });
            foreach (var arr in sourceArrays)
            {
                //将BlockingCollection实例标记为不任何更多的添加。
                //重复值不允许添加到BlockingCollection中
                arr.CompleteAdding();
            }
        }
        static int GetRandomNumber(int seed)
        {
            return new Random(seed).Next(500);
        }

        #region 类-Class
        class PiplineWorker<TInput, TOutput>
        {
            public string Name { get; private set; }

            public BlockingCollection<TOutput>[] Output { get; private set; }

            Func<TInput, TOutput> _processor;
            Action<TInput> _outputProcessor;
            BlockingCollection<TInput>[] _input;

            CancellationToken _token;
            Random _rnd;

            public PiplineWorker(BlockingCollection<TInput>[] input, Func<TInput, TOutput> processor, CancellationToken token, string name)
            {

                _input = input;
                Output = new BlockingCollection<TOutput>[_input.Length];
                for (int i = 0; i < Output.Length; i++)
                {
                    Output[i] = input[i] == null ? null : new BlockingCollection<TOutput>(Count);
                }
                _processor = processor;
                _token = token;
                Name = name;
                _rnd = new Random(DateTime.Now.Millisecond);
            }
            public PiplineWorker(BlockingCollection<TInput>[] input, Action<TInput> render, CancellationToken token, string name)
            {
                _input = input;
                _outputProcessor = render;
                _token = token;
                Name = name;
                Output = null;
                _rnd = new Random(DateTime.Now.Millisecond);
            }
            public void Run()
            {
                WriteLine($"{Name} is runing!");
                //确定是否对序列中的所有元素都满足条件
                while (!_input.All(bc => bc.IsCompleted) && !_token.IsCancellationRequested)
                {
                    TInput receivedItem;
                    //尝试从任一指定的BlockingCollection实例中移除一个项。
                    //从其中一个集合中移除的项。 
                    //等待的毫秒数，或为 System.Threading.Timeout.Infinite (-1)，表示无限期等待。
                    //要观察的取消标记。
                    //从其中移除项的集合在 collections 数组中的索引；如果未能移除项，则为 -1。
                    int i = BlockingCollection<TInput>.TryTakeFromAny(_input, out receivedItem, 50, _token);
                    //移除成功
                    if (i >= 0)
                    {
                        if (Output != null)
                        {
                            TOutput outputItem = _processor(receivedItem);
                            BlockingCollection<TOutput>.AddToAny(Output, outputItem);
                            WriteLine($"{Name} sent {outputItem} to next,on"
                                + $"Thread id {CurrentThread.ManagedThreadId}");
                            Sleep(TimeSpan.FromMilliseconds(_rnd.Next(200)));
                        }
                        else
                        {
                            _outputProcessor(receivedItem);
                        }

                    }
                    else
                    {
                        Sleep(TimeSpan.FromMilliseconds(50));
                    }
                }
                if (Output != null)
                {
                    foreach (var bc in Output)
                    {
                        bc.CompleteAdding();
                    }
                }

            }

        }
        #endregion

        #endregion

        #region 05-28之前 函数支持
        static IEnumerable<int> EnumerableEventSequence()
        {
            for (int i = 0; i < 10; i++)
            {
                Sleep(TimeSpan.FromSeconds(0.5));
                yield return i;
            }
        }




        static void PrintInfo(string typeName)
        {
            Sleep(TimeSpan.FromMilliseconds(150));
            WriteLine($"{typeName} type was printed on a thread" + $"id:{CurrentThread.ManagedThreadId}");
        }

        static IEnumerable<string> GetTypes()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from type in assembly.GetExportedTypes()
                   where type.Name.StartsWith("Web")
                   orderby type.Name.Length
                   select type.Name;
        }
        static string EmulateProcessing(string taskName)
        {
            Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));
            WriteLine($"{taskName} task was processed on a" + $"TheadId is {CurrentThread.ManagedThreadId}");
            return taskName;
        }
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
            string[] rs = await Task.WhenAll(t1, t2, t3, t4, t5, t6, t8);
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
#endregion

        static void Main(string[] args)
        {
            //.SubscribeOn(NewThreadScheduler.Default)
            #region 使用SubscribeOn控制订阅（subscribing）的上下文
            WriteLine("Starting on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            var source = Observable.Create<int>(
            s =>
            {
                WriteLine("Invoked on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                s.OnNext(1);
                s.OnNext(2);
                s.OnNext(3);
                s.OnCompleted();
                WriteLine("Finished on threadId:{0}",
                CurrentThread.ManagedThreadId);
                return Disposable.Empty;
            });
            source
            .SubscribeOn(CurrentThreadScheduler.Instance)
            .Subscribe(
            s => WriteLine("Received {1} on threadId:{0}",
            CurrentThread.ManagedThreadId,
            s),
            () => WriteLine("OnCompleted on threadId:{0}",
            CurrentThread.ManagedThreadId));
            WriteLine("Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            Read();
            #endregion

            #region Rx默认工作模式探索
            WriteLine("Starting on threadId:{0}", CurrentThread.ManagedThreadId);
            var sub = new Subject<Object>();
            //订阅元素处理程序到可观察序列。
            sub.Subscribe(oo => WriteLine("Received {1} on threadId:{0}",//为Observable订阅处理器（handler）输出handler thread id
                CurrentThread.ManagedThreadId,
                oo));
            ParameterizedThreadStart notify = obj =>//委托定义，其内输出被观察对象的thread id
            {
                //打印放进队列的value
                WriteLine("OnNext({1}) on threadId:{0}",
                CurrentThread.ManagedThreadId,
                obj);
                //在序列中通知所有订阅的观察者指定元素的到达情况
                //obj为传递到序列的新函数
                sub.OnNext(obj);
            };
            notify(1);
            new Thread(notify).Start(2);
            new Thread(notify).Start(3);
            WriteLine("Enter to add new value to queue");
            ReadLine();
            new Thread(notify).Start(5);
            ReadLine();
            #endregion

            #region 0610使用TPL数据流库来实现并行管道
            var TPLDemp = ProcessAsyncChronous();
            //通知所有等待线程，结束等待，开始执行
            TPLDemp.GetAwaiter().GetResult();
            WriteLine($"使用TPL数据流库来实现并行管道运行完成！");
            ReadLine();
            #endregion

            #region 使用BlockingCollection实现并行管道
            var cts = new CancellationTokenSource();
            Task.Run(() => 
            {
                if (ReadKey().KeyChar == 'c')
                {
                    cts.Cancel();
                }
            },cts.Token);

            var sourceArrays = new BlockingCollection<int>[CollectionNumber];
            for (int i = 0; i < sourceArrays.Length; i++)
            {
                sourceArrays[i] = new BlockingCollection<int>(Count);
            }
            //Func有参有返回值的委托方法
            var convertToDecimal = new PiplineWorker<int, decimal>(sourceArrays,
                n => Convert.ToDecimal(n * 100),
                cts.Token, "Decimal Converter");
            //Action有参无返回值委托方法
            var stringifyNumber = new PiplineWorker<decimal, string>(convertToDecimal.Output,
                s => WriteLine($"--{s.ToString("C", CultureInfo.GetCultureInfo("en-us"))}"),
                cts.Token, "String Formatter");

            var outputResultToConsole = new PiplineWorker<string, string>(stringifyNumber.
                Output,s => WriteLine($"The final result is {s} on thread"+$"{CurrentThread.ManagedThreadId}"),
                cts.Token,"Console Output");

            try
            {
                Parallel.Invoke(
                    () => CreateInitialValues(sourceArrays, cts),
                    () => convertToDecimal.Run(),
                    () => stringifyNumber.Run(),
                    () => outputResultToConsole.Run()
                    );
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    WriteLine(ex.Message + ex.StackTrace);
                }
            }
            if (cts.Token.IsCancellationRequested)
            {
                WriteLine("Operation has been canceled! Press Enter to exit.");
            }
            else
            {
                WriteLine("Press Enter to exit.");
            }
            WriteLine("使用BlockingCollection实现并行管道Demo执行完成........");
            ReadLine();

            #endregion

            #region 普通遍历与推送订阅模式(基于Rx库)对比
            //-----普通遍历与推送订阅模式对比---------
            foreach (var e in EnumerableEventSequence())
            {
                Write(e);
            }
            WriteLine();
            WriteLine("IEnumberable");
            IObservable<int> o = EnumerableEventSequence().ToObservable();
            using (IDisposable d = o.Subscribe(Write))
            {
                WriteLine();
                WriteLine("IObservable");
            }
            o = EnumerableEventSequence().ToObservable().SubscribeOn(TaskPoolScheduler.Default);
            using (IDisposable subscription = o.Subscribe(Write))
            {
                WriteLine();
                WriteLine("IObservable async");
                ReadLine();
            }
            WriteLine("-------------");
            WriteLine("Rx库使用示例-----1");
            #endregion

            #region PLinq并行查询--Parallel并行执行函数 05-28之前
            ReadLine();
            var PLinqDemo = from t in GetTypes().AsParallel() select EmulateProcessing(t);
            var ppcts = new CancellationTokenSource();
            ppcts.CancelAfter(TimeSpan.FromSeconds(3));
            try
            {
                //调整并行查询参数
                PLinqDemo.WithDegreeOfParallelism(Environment.ProcessorCount)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .WithMergeOptions(ParallelMergeOptions.Default)
                    .WithCancellation(ppcts.Token)
                    .ForAll(WriteLine);
            }
            catch (OperationCanceledException e)
            {
                WriteLine("-------------");
                WriteLine("Operation has been canceled.");
            }
            //-------------------------------------------------------------
            WriteLine("-------------");
            WriteLine("Unordered PLINQ query execution.");
            //无序并行查询
            var unOrderderQuery = from t in ParallelEnumerable.Range(1, 30) select t;
            foreach (var item in unOrderderQuery)
            {
                WriteLine(item);
            }
            //-------------------------------------------------------------
            WriteLine("-------------");
            WriteLine("ordered PLINQ query execution.");
            //顺序并行查询
            var orderedQuery = from t in ParallelEnumerable.Range(1, 30).AsOrdered() select t;
            foreach (var item in orderedQuery)
            {
                WriteLine(item);
            }
            WriteLine("-------------调整Parallel并行查询参数到此结束！-----------------");
            ReadLine();
            //-------------------------------------------------------------
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //方式1
            //单例查询 速度最慢 单例遍历 
            var Query = from t in GetTypes() select EmulateProcessing(t);
            foreach (string typeName in Query)
            {
                PrintInfo(typeName);
            }
            sw.Stop();
            WriteLine("-------------");
            WriteLine("Sequential Linq Query.");
            WriteLine($"Time elspsed:{sw.Elapsed}");
            WriteLine("Please Enter to continue...");
            ReadLine();
            sw.Reset();
            //-----------------------------------------------------------------------------
            sw.Start();
            //方式2
            //查询并行化，结果单线程遍历
            var parallelQuery = from t in GetTypes().AsParallel()//启用查询并行化
                                select EmulateProcessing(t);
            foreach (string typeName in parallelQuery)
            {
                PrintInfo(typeName);
            }
            sw.Stop();
            WriteLine("-------------");
            WriteLine($"Paralle Linq Query.The results are being merged on a single thread");
            WriteLine($"Time elspsed:{sw.Elapsed}");
            WriteLine("Please Enter to continue...");
            ReadLine();
            sw.Reset();
            //-----------------------------------------------------------------------------
            sw.Start();
            //方式3
            //查询并行 遍历并行
            parallelQuery = from t in GetTypes().AsParallel() select EmulateProcessing(t);
            //使用指定的操作为每个元素并行调用source
            //遍历并行
            parallelQuery.ForAll(PrintInfo);
            sw.Stop();
            WriteLine("-------------");
            WriteLine("Parallel Linq Query.The result are being processed in parallel");
            WriteLine($"Time elspsed:{sw.Elapsed}");
            WriteLine("Please Enter to continue...");
            ReadLine();
            sw.Reset();
            //-----------------------------------------------------------------------------
            sw.Start();
            //方式4
            //并行运算强制转换成顺序运算，单例遍历，效率最低
            Query = from t in GetTypes().AsParallel().AsSequential() select EmulateProcessing(t);
            foreach (string typeName in Query)
            {
                PrintInfo(typeName);
            }
            sw.Stop();
            WriteLine("-------------");
            WriteLine("Parallel Linq Query.transformed into sequential");
            WriteLine($"Time elspsed:{sw.Elapsed}");
            WriteLine("Please Enter to continue...");
            ReadLine();




            WriteLine("-------------Parallel并行查询到此结束！-----------------");
            ReadLine();
            //尽可能并行执行提供的每个操作
            //NameSpace:System.Threading.Tasks
            Parallel.Invoke(() => EmulateProcessing("Task1"),
                () => EmulateProcessing("Task2"),
                () => EmulateProcessing("Task3")
                );

            var pcts = new CancellationTokenSource();
            //并行迭代
            var result = Parallel.ForEach(Enumerable.Range(1, 30),

                new ParallelOptions() { CancellationToken = pcts.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    TaskScheduler = TaskScheduler.Default },

                (i, state) =>
                  {
                      WriteLine(i);
                      if (i == 20)
                      {
                          //告知 System.Threading.Tasks.Parallel 循环应在系统方便的时候尽早停止执行当前迭代之外的迭代。
                          state.Break();
                          WriteLine($"Loop is Stop:{state.IsStopped}");
                      }
                  });
            WriteLine("-------------");
            //获取指示循环已完成运行，以便所有的循环迭代期间执行，并且该循环没有收到提前结束的请求。
            WriteLine($"Is Completed:{result.IsCompleted}");
            //从中获取的最低迭代索引 System.Threading.Tasks.ParallelLoopState.Break 调用。
            WriteLine($"Lowest break iteration:{result.LowestBreakIteration}");

            WriteLine("-------------||-----------------");
            ReadLine();
            Task tq = MyAsyncWithAwaitQuickly();
            tq.Wait();
            //Task tl = MyAsyncWithAwaitLow();
            //tl.Wait();

            WriteLine("------------------------------");
            ReadLine();
            Task t1 = AsyncLambda();
            t1.Wait();
            #endregion 

        }

        
        
    }
    

}
