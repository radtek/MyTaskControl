﻿/*===================================================
* 类名称: Http
* 类描述: Http请求类
* 创建人: 李先锋
* 创建时间: 2019/4/2 10:19:07
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtility.NetWork
{
    /// <summary>
    /// Net-Http-Common
    /// </summary>
    public static class HttpUtility
    {

        #region Http请求
        /// <summary>
        /// Http请求
        /// </summary>
        /// <param name="rootURL">请求地址</param>
        /// <param name="ParamsType">参数类型</param>
        /// <param name="queryParams">地址栏参数</param>
        /// <param name="bodyParamStr">Body参数 可为Json/Text</param>
        /// <param name="encoding">URL解码编码</param>
        /// <param name="netType">POST/GET</param>
        /// <param name="charset">Http编码规范</param>
        /// <param name="contentType">请求体类型</param>
        /// <returns></returns>
        public static string HttpReq(string rootURL,ParamsType paramsType, string bodyParamStr, Encoding encoding, NetType netType, ContentType contentType,string charset,IDictionary queryParams = null)
        {
            string result = string.Empty;
            HttpWebRequest req = null;
            switch (paramsType)
            {
                case ParamsType.Header:
                    req = (HttpWebRequest)WebRequest.Create($"{rootURL}");
                    req.Headers.Add(GetWebHeaderCollection(queryParams));
                    break;
                case ParamsType.QueryParams:
                    req = (HttpWebRequest)WebRequest.Create($"{rootURL}?{KeyValueHandle(queryParams)}");
                    break;
                case ParamsType.NoParams:
                    req = (HttpWebRequest)WebRequest.Create($"{rootURL}");
                    break;
                default:
                    break;
            }
            req.Method = GetNetType(netType);
            req.ContentType = $"{GetContentType(contentType)}{charset}";
            #region 添加Post 参数
            byte[] data = encoding.GetBytes(bodyParamStr);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            //URL转码
            return System.Web.HttpUtility.UrlDecode(result, encoding);
        }
        /// <summary>
        /// Http请求
        /// </summary>
        /// <param name="rootURL">请求地址</param>
        /// <param name="ParamsType">参数类型</param>
        /// <param name="queryParams">地址栏参数</param>
        /// <param name="bodyParamStr">Body参数 可为Json/Text</param>
        /// <param name="encoding">URL解码编码</param>
        /// <param name="netType">POST/GET</param>
        /// <param name="Charset">Http编码规范</param>
        /// <param name="contentType">请求体类型</param>
        /// <returns></returns>
        public static string HttpReq(string rootURL, ParamsType paramsType, string bodyParamStr, Encoding encoding, NetType netType, ContentType contentType, Charset charset, IDictionary queryParams = null)
        {
            string result = string.Empty;
            HttpWebRequest req = null;
            switch (paramsType)
            {
                case ParamsType.Header:
                    req = (HttpWebRequest)WebRequest.Create($"{rootURL}");
                    req.Headers.Add(GetWebHeaderCollection(queryParams));
                    break;
                case ParamsType.QueryParams:
                    req = (HttpWebRequest)WebRequest.Create($"{rootURL}?{KeyValueHandle(queryParams)}");
                    break;
                case ParamsType.NoParams:
                    req = (HttpWebRequest)WebRequest.Create($"{rootURL}");
                    break;
                default:
                    break;
            }
            req.Method = GetNetType(netType);
            if (charset == Charset.UTF_8)
            {
                req.ContentType = $"{GetContentType(contentType)}{"utf-8"}";
            }
            else
            {
                req.ContentType = $"{GetContentType(contentType)}{charset}";
            }
            #region 添加Post 参数
            if (netType == NetType.POST)
            {
                byte[] data = encoding.GetBytes(bodyParamStr);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            //URL转码
            return System.Web.HttpUtility.UrlDecode(result, encoding);
        }
        #endregion

        #region Http枚举
        /// <summary>
        /// 编码方式
        /// </summary>
        public enum Charset
        {
            UTF_8 = 0,
            Uncicode = 1,
            GB2312 = 2
        }
        /// <summary>
        /// HttpRequestType
        /// </summary>
        public enum NetType
        {
            POST = 1,
            GET = 2,
            PUT = 3,
            DELETE = 4
        }
        /// <summary>
        /// HttpContentType
        /// </summary>
        public enum ContentType
        {
            Application_WWW = 1,
            Application_Json = 2,
            textplain = 3,
            textxml = 4,
            textjson = 5
        }
        /// <summary>
        /// 请求参数类型
        /// </summary>
        public enum ParamsType
        {
            Header = 0,
            QueryParams = 1,
            NoParams = 2
        }
        #endregion

        #region Http枚举解析
        /// <summary>
        /// 获取请求类型
        /// </summary>
        /// <param name="netType"></param>
        /// <returns></returns>
        private static string GetNetType(NetType netType)
        {
            switch (netType)
            {
                case NetType.POST:
                    return "POST";
                case NetType.GET:
                    return "GET";
                case NetType.DELETE:
                    return "DELETE";
                case NetType.PUT:
                    return "PUT";
                default:
                    return null;
            }
        }
        /// <summary>
        /// 请求体格式
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static string GetContentType(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Application_WWW:
                    return "application/x-www-form-urlencoded;";
                case ContentType.Application_Json:
                    return "application/json;";
                case ContentType.textplain:
                    return "text/plain";
                case ContentType.textjson:
                    return "text/json;";
                case ContentType.textxml:
                    return "text/xml;";
                default:
                    return null;
            }
        }
        #endregion

        #region Http辅助方法
        /// <summary>
        /// 忽略Null值序列化Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJsonIgnoreNull(object obj)
        {
            //更多序列化/反序列化设置，可以通过显示设置JsonSerializerSettings枚举/属性来设置格式
            JsonSerializerSettings mJsonSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(obj, Formatting.Indented, mJsonSettings);
        }
        /// <summary>
        /// 键值对请求参数拼接
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string KeyValueHandle(IDictionary param)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in param.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(key + "=" + param[key]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Http请求头参数添加
        /// </summary>
        /// <param name="HeaderParameter"></param>
        /// <returns></returns>
        private static WebHeaderCollection GetWebHeaderCollection(IDictionary HeaderParameter)
        {
            WebHeaderCollection webHeader = new WebHeaderCollection();
            foreach (string key in HeaderParameter.Keys)
            {
                webHeader.Add(key, HeaderParameter[key].ToString());
            }
            return webHeader;
        }
        #endregion

    }
}
