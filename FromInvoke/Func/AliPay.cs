/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：FromInvoke.Func
*   文件名称    ：AliPay.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/19 16:11:12 
*   功能描述    ：模拟支付宝交易业务方法
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
using FromInvoke.Interface;
using FromInvoke.Model;

namespace FromInvoke.Func
{
    public class AliPay : TradeHandle
    {
        public void Handle()
        {
            PaySuccess result = new PaySuccess
            {
                IsSuccess = true,
                Amount = "0.01",
                ErrorMsg = "",
                PaySubWay = "AliPay",
                ThirdNo = "Ali201944657987"
            };
            System.Threading.Thread.Sleep(5000);
            //固定写法
            TradeCallBack(result);
            TradeFinish();
        }
    }
}
