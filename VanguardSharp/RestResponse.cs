/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：VanguardSharp
*   文件名称    ：RestResponse.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/12 16:36:22 
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
using VanguardSharp.Interface;

namespace VanguardSharp
{
    public class RestResponse<T> : IRestResponse<T>
    {
        public T Data { get; set; }
        public int HttpStatusCode { get ; set ; }
        public Exception RestError { get; set; }
        public string ErrMsg { get; set; }
    }
}
