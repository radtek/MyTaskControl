/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：PosSharp.FacadeLog
*   文件名称    ：FacadeLogUtility.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/25 10:06:31 
*   功能描述    ：三方支付通用FacadeLog记录
*   使用说明    ：
*   =================================
*   修改者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PosSharp.FacadeLog
{
    public static class FacadeLogUtility
    {
        public static string UtilityLog(this Romens.RCP.NET.NetAccesser accesser, string userGuid,LogRecord log)
        {
            string logStr = JsonConvert.SerializeObject(log);
            return accesser.DoPostBack("ThirdTradeLog", userGuid, "UtilityLog", logStr)?.ToString();
        }
    }
}
