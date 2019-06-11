using LiFFHelper.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiFFHelper.Utility
{
    /// <summary>
    /// 通用类
    /// </summary>
    public static class RestfulUtility
    {
        /// <summary>
        /// 农银e管家 请求URL拼接
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="ControlName"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static string AgeUrlFormat(string rootUrl, string ControlName, string MethodName)
        {
            return $"{rootUrl}/{ControlName}/{MethodName}";
        }
        public static string CreateOrderNo()
        {
            string[] Abc = { "A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "H", "h" };
            StringBuilder tags = new StringBuilder();
            Random ran = new Random();
            for (int i = 0; i < 5; i++)
            {
                tags.Append(Abc[ran.Next(0, 14)]);
            }
            return $"{tags.ToString()}{DateTime.Now.CommonDateTimap(SafeExtendFormat.DateTimap.Common)}";
        }
    }
}
