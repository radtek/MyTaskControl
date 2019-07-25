/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：PosSharp.FacadeLog
*   文件名称    ：LogRecord.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/7/1 13:53:33 
*   功能描述    ：交易Log对象类
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

namespace PosSharp.FacadeLog
{
    public class LogRecord
    {
        /// <summary>
        /// 日志表GUID
        /// </summary>
        public string LogGuid { get; set; }
        /// <summary>
        /// 函数名称
        /// </summary>
        public string FuncName { get; set; }
        /// <summary>
        /// Erp订单号
        /// </summary>
        public string ErpBillNo { get; set; }
        /// <summary>
        /// 第三方支付方式
        /// </summary>
        public string PaySubWay { get; set; }
        /// <summary>
        /// 第三方支付平台的订单号
        /// </summary>
        public string ThirdBillNo { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 请求业务参数
        /// </summary>
        public object RequestBusinessParams { get; set; }
        /// <summary>
        /// 响应结果
        /// </summary>
        public object ResponseResult { get; set; }
        /// <summary>
        /// 当前单据状态
        /// </summary>
        public BillStatus Status { get; set; } = BillStatus.TRADING;
        /// <summary>
        /// 订单状态
        /// </summary>
        public enum BillStatus
        {
            PAID = 0,
            CANCELED = 1,
            REFUNDED = 2,
            ERROR = 3,
            TRADING = 4
        }

        public string ErrorMsg { get; set; }

        public string ErrorType { get; set; }

    }

}
