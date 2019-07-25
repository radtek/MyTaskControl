/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：FromInvoke.Model
*   文件名称    ：PaySuccess.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/19 16:00:20 
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

namespace FromInvoke.Model
{
    public class PaySuccess
    {
        public string ThirdNo { get; set; }

        public string Amount { get; set; }

        public string PaySubWay { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMsg { get; set; }
    }
}
