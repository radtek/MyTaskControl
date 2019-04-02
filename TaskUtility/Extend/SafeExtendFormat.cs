/*===================================================
* 类名称: SafeFormat
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/3/18 17:19:10
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TaskUtility.Extend
{
    /// <summary>
    /// StringExtendClass
    /// 字符串功能扩展类
    /// </summary>
    public static class SafeExtendFormat
    {
        
        private static readonly TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// 13位时间戳（毫秒）
        /// </summary>
        public static string LocalTimeStampLong => Convert.ToInt64(ts.TotalMilliseconds).ToString();
        /// <summary>
        /// 10位时间戳（秒）
        /// </summary>
        public static string LocalTimeStampInt => Convert.ToInt64(ts.TotalSeconds).ToString();
        /// <summary>
        /// 转换成decimal
        /// ----
        /// Convert false will be default 0
        /// </summary>
        /// <param name="decimalstr"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string decimalstr)
        {
            if (string.IsNullOrEmpty(decimalstr))
                return 0;
            else
            {
                if (decimal.TryParse(decimalstr, out decimal result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 转换成float
        /// ----
        /// Convert false will be default 0
        /// </summary>
        /// <param name="decimalstr"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            else
            {
                if (float.TryParse(str,out float result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// SQL特殊敏感字符替换
        /// </summary>
        /// <returns></returns>
        public static string SafeSqlReplace(this string txt)
        {
            if (!IsSafeSqlString(txt))
            {
                return SQLSafe(StripSQLInjection(txt));
            }
            else
            {
                return txt;
            }
        }
        /// <summary>
        /// 敏感字符判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>  
        /// 删除SQL注入特殊字符  
        /// 解然 20070622加入对输入参数sql为Null的判断  
        /// </summary>  
        private static string StripSQLInjection(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                //过滤 ' --  
                string pattern1 = @"(\%27)|(\')|(\-\-)";

                //防止执行 ' or  
                string pattern2 = @"((\%27)|(\'))\s*((\%6F)|o|(\%4F))((\%72)|r|(\%52))";

                //防止执行sql server 内部存储过程或扩展存储过程  
                string pattern3 = @"\s+exec(\s|\+)+(s|x)p\w+";

                sql = Regex.Replace(sql, pattern1, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern2, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern3, string.Empty, RegexOptions.IgnoreCase);
            }
            return sql;
        }
        /// <summary>
        /// 替换<>
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public static string SQLSafe(string Parameter)
        {
            Parameter = Parameter.ToLower();
            Parameter = Parameter.Replace("'", "");
            Parameter = Parameter.Replace(">", ">");
            Parameter = Parameter.Replace("<", "<");
            Parameter = Parameter.Replace("\n", "<br>");
            Parameter = Parameter.Replace("\0", "·");
            return Parameter;
        }
        /// <summary>
        /// SQL语句拼接（防注入）
        /// 目前支持string、int、decimal、float、double、long参数
        /// </summary>
        /// <param name="sqltxt"></param>
        /// <param name="type"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public static string SafeSQLFormat(this string sqltxt,SQLType type,params object[] sqlParams)
        {
            string sqlLabel = ":";
            if (type == SQLType.SqlServer)
            {
                sqlLabel = "@";
            }
            int sqlParam = sqlParams.Length;
            if (string.IsNullOrEmpty(sqltxt))
                return "";
            StringBuilder formatResult = new StringBuilder(sqltxt);
            for(int i = 0; i < sqlParams.Length; i++)
            {
                object SafeNum = 0;
                if (sqlParams[i] is string && !string.IsNullOrEmpty(sqlParams[i].ToString()))
                {
                    string safeParamStr = sqlParams[i].ToString();
                    if (!IsSafeSqlString(safeParamStr))
                    {
                        //替换敏感字符
                        safeParamStr = SQLSafe(StripSQLInjection(safeParamStr));
                    }
                    //对位替换
                    formatResult.Replace($"{sqlLabel}{i}",$"'{safeParamStr}'" );
                }
                if (sqlParams[i] != null && sqlParams[i] is decimal)
                {
                    if (decimal.TryParse(sqlParams[i].ToString(),out decimal result))
                    {
                        //对位替换
                        formatResult.Replace($"{sqlLabel}{i}", result.ToString());
                    }
                    else
                    {
                        //非法值对位替换
                        formatResult.Replace($"{sqlLabel}{i}", SafeNum.ToString());
                    }
                    
                }
                if (sqlParams[i] != null && sqlParams[i] is float)
                {
                    if (float.TryParse(sqlParams[i].ToString(), out float result))
                    {
                        //对位替换
                        formatResult.Replace($"{sqlLabel}{i}", result.ToString());
                    }
                    else
                    {
                        //非法值对位替换
                        formatResult.Replace($"{sqlLabel}{i}", SafeNum.ToString());
                    }
                }
                if (sqlParams[i] != null && sqlParams[i] is double)
                {
                    if (double.TryParse(sqlParams[i].ToString(), out double result))
                    {
                        //对位替换
                        formatResult.Replace($"{sqlLabel}{i}", result.ToString());
                    }
                    else
                    {
                        //非法值对位替换
                        formatResult.Replace($"{sqlLabel}{i}", SafeNum.ToString());
                    }
                }
                if (sqlParams[i] != null && sqlParams[i] is int)
                {
                    if (int.TryParse(sqlParams[i].ToString(), out int result))
                    {
                        //对位替换
                        formatResult.Replace($"{sqlLabel}{i}", result.ToString());
                    }
                    else
                    {
                        //非法值对位替换
                        formatResult.Replace($"{sqlLabel}{i}", SafeNum.ToString());
                    }
                }
                if (sqlParams[i] != null && sqlParams[i] is long)
                {
                    if (long.TryParse(sqlParams[i].ToString(), out long result))
                    {
                        //对位替换
                        formatResult.Replace($"{sqlLabel}{i}", result.ToString());
                    }
                    else
                    {
                        //非法值对位替换
                        formatResult.Replace($"{sqlLabel}{i}", SafeNum.ToString());
                    }
                }
                else
                {
                    //对位替换
                    formatResult.Replace($"{sqlLabel}{i}", sqlParams[i]?.ToString());
                }
            }
            return formatResult.ToString();
        }
        /// <summary>
        /// SQL语句类型
        /// </summary>
        public enum SQLType
        {
            /// <summary>
            /// 格式为：@0
            /// </summary>
            SqlServer = 0,
            /// <summary>
            /// 格式为：:1
            /// </summary>
            Oracle = 1
        }
        /// <summary>
        /// 提取安全的String
        /// 支持字母、数字、汉字、常用标点符号
        /// </summary>
        /// <param name="Sqltxt"></param>
        /// <returns></returns>
        public static string ExtractSafeSqlTxt(string Sqltxt)
        {
            Regex rule = new Regex(@"[a-zA-Z0-9\u4e00-\u9fa5，,.。?？!！]+");
            string r = string.Empty;
            foreach (Match mch in rule.Matches(Sqltxt))
            {
                r = r + mch.Value.Trim();
            }
            return r;
        }
        /// <summary>
        /// 日期转换
        /// 结果示例：2019年03月18日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ChineseDateFormat(this DateTime date)
        {
            return $"{date:D}";
        }
        /// <summary>
        /// 时间转换
        /// 结果格式为：15:41
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ShortTimeFormat(this DateTime date)
        {
            return $"{date:t}";
        }
        /// <summary>
        /// 常用长时间 格式
        /// </summary>
        /// <param name="date"></param>
        /// <param name="fromattype"></param>
        /// <returns></returns>
        public static string CommonDateTimap(this DateTime date,DateTimap fromattype)
        {
            switch (fromattype)
            {
                case DateTimap.Diagonal:
                    return date.ToString("yyyy/MM/dd HH:mm:ss");
                case DateTimap.Horizontal:
                    return $"{date:G}";
                default:
                    return $"{date:G}";
            }
            
        }
        /// <summary>
        /// 长时间格式
        /// </summary>
        public enum DateTimap
        {
            /// <summary>
            /// 斜线
            /// </summary>
            Diagonal = 0,
            /// <summary>
            /// 横线
            /// </summary>
            Horizontal = 1

        }
        /// <summary>
        /// 时间戳类型
        /// </summary>
        public enum TimeStampType
        {
            /// <summary>
            /// 10位时间戳
            /// </summary>
            Ten = 0,
            /// <summary>
            /// 13位时间戳
            /// </summary>
            Thirteen = 1
        }
        /// <summary>
        /// 时间戳转DateTime
        /// </summary>
        /// <param name="timestr"></param>
        /// <returns></returns>
        public static DateTime GetTimeFromTimeStampInt(this string timestr,TimeStampType type)
        {
            DateTime dtStart;
            long lTime;
            TimeSpan toNow;
            switch (type)
            {
                case TimeStampType.Ten:
                    dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    lTime = long.Parse(timestr + "0000000");
                    toNow = new TimeSpan(lTime);
                    return dtStart.Add(toNow);
                case TimeStampType.Thirteen:
                    dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    lTime = long.Parse(timestr + "0000");
                    toNow = new TimeSpan(lTime);
                    return dtStart.Add(toNow); ;
                default:
                    return DateTime.Now;
            }
        }
        /// <summary>
        /// 验证时间格式是否正确
        /// </summary>
        /// <param name="input">时间串</param>
        /// <param name="dateformat">格式，例如：yyyy-MM-dd</param>
        /// <returns></returns>
        public static bool CheakDate(this string input, string dateformat)
        {
            return DateTime.TryParseExact(input, dateformat, null, System.Globalization.DateTimeStyles.None, out DateTime date);
        }
        /// <summary>
        /// 验证时间格式并返回DateTime
        /// </summary>
        /// <param name="input">时间串</param>
        /// <param name="dateformat">格式，例如：yyyy-MM-dd</param>
        /// <param name="date">DateTime结果</param>
        /// <returns></returns>
        public static bool CheakDate(this string input, string dateformat, out DateTime date)
        {
            return DateTime.TryParseExact(input, dateformat, null, System.Globalization.DateTimeStyles.None, out date);
        }
    }
}
