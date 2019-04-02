/*===================================================
* 类名称: TaskException
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/3/29 17:22:26
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtility.Common
{
    /// <summary>
    /// 作业处理错误结果对象类
    /// </summary>
    public class TaskException
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskNo { get; set; }
        /// <summary>
        /// 任务子编号
        /// </summary>
        public string TaskSubNo { get; set; }
        /// <summary>
        /// 作业类型
        /// </summary>
        public enum TaskType
        {
            /// <summary>
            /// 上传作业
            /// </summary>
            UpTask = 0,
            /// <summary>
            /// 下载作业
            /// </summary>
            DownTask =1,
            /// <summary>
            /// 平台自作业
            /// </summary>
            PlatformTask
        }
        /// <summary>
        /// 错误级别
        /// </summary>
        public enum ErrorLevel
        {
            /// <summary>
            /// 常见错误
            /// </summary>
            Common = 0,
            /// <summary>
            /// 可恢复错误
            /// </summary>
            Uecoverable = 1,
            /// <summary>
            /// 严重错误
            /// </summary>
            Fatal = 2,
        }
        /// <summary>
        /// 错误行号
        /// </summary>
        public string ErrorLineNo { get; set; }
        /// <summary>
        /// 错误类型
        /// </summary>
        public Type ErrorType { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 错误Group
        /// </summary>
        public Exception[] ErrorGroup { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public string LogData = $"{DateTime.Now:F}";
    }
}
