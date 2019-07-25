/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：TaskDemo.Rx
*   文件名称    ：MySequenceOfNumbers.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/27 15:48:48 
*   功能描述    ：
*   使用说明    ：
*   =================================
*   修改者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo.Rx
{
    public class MySequenceOfNumbers: IObservable<int>
    {
        public IDisposable Subscribe(IObserver<int> observer)
        {
            //向观察者提供新数据
            observer.OnNext(1);
            observer.OnNext(2);
            observer.OnNext(3);
            //数据已经放完，可以开始观察处理了。
            //通知观察者提供程序已完成发送基于推送的通知。
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
