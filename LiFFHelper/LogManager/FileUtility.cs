/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：LiFFHelper.LogManager
*   文件名称    ：FileReader.cs
*   =================================
*   创 建 者    ：李先锋
*   创建日期    ：2019/6/11 14:07:09 
*   功能描述    ：文件操作功能类
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

namespace LiFFHelper.LogManager
{
    public static class FileUtility
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadFile(string file)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                string content;
                while ((content = sr.ReadLine()) != null)
                {
                    sb.Append(content.ToString());
                }
            }
            return sb.ToString();
        }
    }
}
