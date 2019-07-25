/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：VanguardSharp
*   文件名称    ：RestClient.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/12 16:17:38 
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
using VanguardSharp.Interface;

namespace VanguardSharp
{
    public class RestClient:IRestClient
    {
        private string RestUrl = string.Empty;
        public RestClient(string url)
        {
            RestUrl = url;
        }
        public RestClient() { }
        /// <summary>
        /// URL地址
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Https证书
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }
        /// <summary>
        /// 请求编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public int Timeout { get; set; } = 500;

        public void SetUrl(string url)
        {
            RestUrl = url;
        }
        public IRestResponse<T> Excute<T>(IRestRequest request)
        {
            RestResponse<T> r = new RestResponse<T>();
            r.ErrMsg = "";
            return r;

        }




    }
}
