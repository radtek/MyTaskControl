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
                xmlTextWriter.Formatting = System.Xml.Formatting.None;
                xmlSerializer.Serialize(xmlTextWriter, obj, namespaces);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
                return encoding.GetString(memoryStream.ToArray());
            }
        }
        public static string XmlSerializeNoHeader<T>(T obj, Encoding encoding)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                //序列化对象
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
                xmlTextWriter.Formatting = System.Xml.Formatting.None;
                xmlSerializer.Serialize(xmlTextWriter, obj, namespaces);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
                string xml = encoding.GetString(memoryStream.ToArray());
                return xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            }
        }

        #endregion
    }
}
