/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：PosSharp.Interface
*   文件名称    ：IPosTradeResult.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/7/8 15:29:36 
*   功能描述    ：Pos函数返回结果对象
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
    public interface IPosTradeResult
    {
        /// <summary>
        /// 应答码
        /// </summary>
        string Code { get; set; }
        /// <summary>
        /// 应答信息
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// 请求数据
        /// </summary>
        object ClientData { get; set; }
        /// <summary>
        /// 响应/处理结果（可自定义Model）
        /// </summary>
        object Data { get; set; }
        /// <summary>
        /// 是否产生错误
        /// </summary>
        bool HasError { get; set; } 
        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrorMessage { get; set; }
        /// <summary>
        /// 错误类型
        /// </summary>
        string ErrorType { get; set; }
    }
}
