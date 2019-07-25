/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：VanguardSharp.Interface
*   文件名称    ：IRestResponse.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/12 16:37:04 
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

namespace VanguardSharp.Interface
{
    public interface IRestResponse <T> 
    {
        T Data { get; set; }

        int HttpStatusCode { get; set; }

        Exception RestError { get; set; }

        string ErrMsg { get; set; }
    }
}
