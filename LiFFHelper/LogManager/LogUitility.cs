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
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<string> GetConfigInfo(string file)
        {
            try
            {
                List<string> config = new List<string>();
                StreamReader sr = new StreamReader(file, Encoding.UTF8);
                string content;
                while ((content = sr.ReadLine()) != null)
                {
                    config.Add(content.ToString());
                }
                return config;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Error(Exception err) { Log(LogTags.Error, null, err); }
        public void Error(string Message) { Log(LogTags.Error, Message); }
        public void Error(string Message, Exception err) { Log(LogTags.Error, Message, err); }
        public void Debug(string Message) { Log(LogTags.Debug, Message); }
        public void Info(string Message) { Log(LogTags.Info, Message); }
        public void Uat(string Message) { Log(LogTags.Uat, Message); }
        public void Request(string Message) { Log(LogTags.Request, Message); }
        public void Response(string Message) { Log(LogTags.Response, Message); }
        public void Test(string Message) { Log(LogTags.Test, Message); }
        public void Log(string Message) { Log(LogTags.Log, Message); }

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
                SW.WriteLine("┏"+"".PadRight(70, '━') +"┓");
                SW.WriteLine("┃" + "".PadRight(10, ' ')+"应用日志文件".PadRight(33,' ')+"┃");
                SW.WriteLine("┣" + "".PadRight(70, '━') + "┫");
                SW.WriteLine("┃" + "".PadRight(10, ' ') + "公司 ：雨人软件".PadRight(32, ' ') + "┃");
                SW.WriteLine("┃" + "".PadRight(10, ' ') + "作者 : 李锋锋".PadRight(34,' ')+"┃");
                SW.WriteLine("┃" + "".PadRight(10, ' ') + "说明 : API接口日志记录".PadRight(31,' ')+"┃");
                SW.WriteLine($"┃" + "".PadRight(10, ' ') + "时间 : "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff").PadRight(32,' ')+"┃");
                SW.WriteLine("┗"+ "".PadRight(70, '━') + "┛");
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
            Uat = 6,
            Log = 7
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
                $"{Environment.NewLine}日志内容：{logInfo??"NULL"}{Environment.NewLine}");
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
