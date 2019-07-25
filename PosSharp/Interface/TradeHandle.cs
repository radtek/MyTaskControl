/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：FromInvoke.Interface
*   文件名称    ：TradeHandle.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/19 17:24:55 
*   功能描述    ：第三方支付集成到Pos父类参数
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
using System.Text;

namespace PosSharp.Interface
{
    public class TradeHandle
    {
        public delegate void ThirdTrade(IPosTradeResult r, TradeFuncType type);
        public ThirdTrade TradeCallBack;

        public delegate void ThirdFinish(IPosTradeResult r, TradeFuncType type);
        public ThirdFinish TradeFinish;
        public enum TradeFuncType
        {
            Pay = 0,
            Query = 1,
            Cancel = 2,
            Refund = 3
        }
        public virtual void Pay(object reqParams)
        {

        }
        public virtual void Query(object reqParams)
        {

        }
        public virtual void Cancel(object reqParams)
        {

        }
        public virtual void Refund(object reqParams)
        {

        }
    }
}
