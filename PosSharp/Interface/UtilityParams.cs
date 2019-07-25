/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：PosSharp.Interface
*   文件名称    ：UtilityParams.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/25 10:38:57 
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
using System.Text;

namespace PosSharp.Interface
{
    //Pos入参Model开发规范
    public class UtilityParams: IPosTradeParams
    {
        /// <summary>
        /// 依赖项地址
        /// </summary>
        public string LibraryFile { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string FuncName { get; set; }

        /// <summary>
        /// 参数对象
        /// </summary>
        public object Param { get; set; }
    }
}
