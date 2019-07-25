/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：VanguardSharp.Interface
*   文件名称    ：IRestClientcs.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/12 17:26:32 
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
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace VanguardSharp.Interface
{
    public interface IRestClient
    {
        /// <summary>
        /// URL地址
        /// </summary>
        string URL { get; set; }
        /// <summary>
        /// Https证书
        /// </summary>
        X509CertificateCollection ClientCertificates { get; set; }
        /// <summary>
        /// 请求编码
        /// </summary>
        Encoding Encoding { get; set; }
        /// <summary>
        /// 请求超时时间
        /// </summary>
        int Timeout { get; set; }

        IRestResponse<T> Excute<T>(IRestRequest request);
    }
}
