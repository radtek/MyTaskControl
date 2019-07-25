/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：PosSharp.Helper
*   文件名称    ：ParamsFormat.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/25 10:27:36 
*   功能描述    ：Json、Xml格式化
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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PosSharp.Helper
{
    public static class ParamsFormat
    {
        #region Xml读取
        public static string GetXmlValue(this string XmlStr,string RootName, string KeyName)
        {
            if (string.IsNullOrEmpty(XmlStr))
            {
                return "";
            }
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.LoadXml(XmlStr);
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
        /// <summary>
        /// 干净Xml序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj, Encoding encoding)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                //序列化对象
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
                xmlTextWriter.Formatting = Formatting.None;
                xmlSerializer.Serialize(xmlTextWriter, obj, namespaces);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
                return encoding.GetString(memoryStream.ToArray());
            }
        }

        #endregion
    }
}
