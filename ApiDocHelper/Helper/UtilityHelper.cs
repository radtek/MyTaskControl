using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiDocHelper.Helper
{
    /// <summary>
    /// Doc公共操作类
    /// </summary>
    public static class UtilityHelper
    {
        /// <summary>
        /// 预览ApiDoc
        /// </summary>
        /// <param name="fileName"></param>
        public static void WordDocView(string fileName)
        {
            System.Diagnostics.Process.Start(fileName);
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="Path">文件地址</param>
        public static string ReadFileContent(string Path)
        {
            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(Path, Encoding.UTF8);
            string content;
            while ((content = sr.ReadLine()) != null)
            {
                sb.Append(content);
            }
            return sb.ToString();
        }
        public static string CreateApiDocNo()
        {
            string[] Abc = { "A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "H", "h" };
            StringBuilder tags = new StringBuilder();
            Random ran = new Random();
            for (int i = 0; i < 5; i++)
            {
                tags.Append(Abc[ran.Next(0, 14)]);
            }
            return $"{tags.ToString()}{DateTime.Now.ToString("yyyyMMddHHmmss")}";
        }

    }
}
