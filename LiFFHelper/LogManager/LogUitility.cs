using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiFFHelper.LogManager
{
    public class LogUitility
    {
        public void Error(Exception err) { Log(LogTags.Error, null, err); }
        public void Error(string Message) { Log(LogTags.Error, Message); }
        public void Debug(string Message) { Log(LogTags.Debug, Message); }
        public void Info(string Message) { Log(LogTags.Info, Message); }
        public void Uat(string Message) { Log(LogTags.Uat, Message); }
        public void Request(string Message) { Log(LogTags.Request, Message); }
        public void Response(string Message) { Log(LogTags.Response, Message); }
        public void Test(string Message) { Log(LogTags.Test, Message); }

        private string LogPath;
        public LogUitility(string path)
        {
             LogPath = path;
        }
        private static readonly object FileLock = new object();
        #region 文件是否存在
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        private static void ExistsFile(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = @"D:\Log";
            }
            if (!File.Exists(FilePath))
            {
                FileStream fso = File.Create(FilePath);
                fso.Close();
                fso.Dispose();
                StreamWriter SW = File.AppendText(FilePath);
                SW.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
                SW.WriteLine("┃                应用日志文件  v1.0                ┃");
                SW.WriteLine("┣━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┫");
                SW.WriteLine("┃         公司 : 雨人软件(www.Romens.com)          ┃");
                SW.WriteLine("┃         作者 : 李锋锋                            ┃");
                SW.WriteLine("┃         说明 : RestFulApi接口日志记录             ┃");
                SW.WriteLine($"┃     创建时间 : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}          ┃");
                SW.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                SW.WriteLine("");
                SW.Flush();
                SW.Close();
                SW.Dispose();
            }
        }
        #endregion
        /// <summary>
        /// 日志标签
        /// </summary>
        public enum LogTags
        {
            Error = 0,
            Debug = 1,
            Info = 2,
            Test = 3,
            Request = 4,
            Response = 5,
            Uat = 6
        }
        public void Log(LogTags tags,string logInfo = null,Exception err = null)
        {
            if (string.IsNullOrEmpty(LogPath))
            {
                LogPath = @"D:\Log";
            }
            //先判断文件夹是否存在并创建
            //目录确认
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            Monitor.Enter(FileLock);
            string FilePath = LogPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + $".{tags.ToString()}";
            ExistsFile(FilePath);
            StreamWriter sw = File.AppendText(FilePath);
            sw.WriteLine("".PadLeft(70, '━'));
            sw.WriteLine($"{Environment.NewLine}记录时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}" +
                $"{Environment.NewLine}日志类型：{tags.ToString()}" +
                $"{Environment.NewLine}日志内容:{logInfo??"未经处理的异常"}{Environment.NewLine}");
            if (tags == LogTags.Error && err != null)
            {
                sw.WriteLine(
                $"错误信息：{err.Message}{Environment.NewLine}" +
                $"错误类型：{err.GetType().ToString()}{Environment.NewLine}" +
                $"调用方法：{err.TargetSite.Name}{Environment.NewLine}" +
                $"NamSpace：{err.TargetSite.DeclaringType.FullName}{Environment.NewLine}" +
                $"错误程序/对象：{err.Source}{Environment.NewLine}" +
                $"异常位置：{err.StackTrace}{Environment.NewLine}");
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
            Monitor.Exit(FileLock);
        }

    }
}
