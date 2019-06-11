using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LiFFHelper.NetWork
{
    public static class MyDataFormat
    {
       
       static XmlDocument XmlDoc = new XmlDocument();
        /// <summary>
        /// 设置XML字符串
        /// </summary>
        /// <param name="XmlStr"></param>
        public static void SetXmlDoc(string XmlStr)
        {
            XmlDoc.LoadXml(XmlStr);
        }
        #region XML格式化
        /// <summary>
        /// 获取XML数据指定节点下的数据
        /// </summary>
        /// <param name="RootName">根节点</param>
        /// <param name="KeyName">需要取值的节点</param>
        /// <param name="XmlStr">XML字符串</param>
        /// <returns></returns>
        public static string GetXmlValue(string RootName,string KeyName)
        {
            if (!XmlDoc.HasChildNodes)
            {
                return "";
            }
            else
            {
                XmlNode root = XmlDoc.SelectSingleNode(RootName);
                return root.SelectSingleNode(KeyName).InnerText;
            }    

        }
        #endregion

        #region XML序列化反序列化
        public static T DESerializer<T>(string strXML) where T : class
        {
            using (StringReader sr = new StringReader(strXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(sr) as T;
            }
        }
        public static string XmlSerialize<T>(T obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(sw, obj, namespaces);
                sw.Close();
                return sw.ToString();
            }
        }
        public static string XmlSerializeFormat<T>(T obj)
        {
            StringBuilder sb = new StringBuilder(XmlSerialize(obj));
            sb.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            sb.Replace("\r\n", "");
            sb.Replace(" ", "");
            return sb.ToString();
        }
        public static string XmlSerializeFormat<T>(T obj,string rootnamespace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(rootnamespace);
            sb.Append(XmlSerialize(obj));
            sb.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            sb.Replace("\r\n", "");
            sb.Replace(" ", "");
            return sb.ToString();
        }

        #endregion
    }
}
