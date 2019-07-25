using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//
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
using RestSharp;
using ApiDocHelper.Model;
using Newtonsoft.Json;
using TaskDemo.Rx;
using Nito.AsyncEx;
using System.Windows.Media;
using System.Timers;

namespace TaskDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var uiContext = SynchronizationContext.Current;
            Trace.WriteLine($"UI thread id is{Environment.CurrentManagedThreadId}");
            Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
                handler => (s,a) => handler(s,a),
                handler => MouseMove += handler,
                handler => MouseMove -= handler
                )
                .Select(evt => evt.EventArgs)
                .ObserveOn(Scheduler.Default)
                .Select(position => {
                    //复杂的计算过程
                    Thread.Sleep(1000);
                    var result = position.X + position.Y;
                    Trace.WriteLine($"Click POsition:{result}|thread id:{Environment.CurrentManagedThreadId}");
                    return result;
                })
                .ObserveOn(uiContext)
                .Subscribe(x => Trace.WriteLine($"Result:{x}|thread id:{Environment.CurrentManagedThreadId}"));

        }
    }
}
