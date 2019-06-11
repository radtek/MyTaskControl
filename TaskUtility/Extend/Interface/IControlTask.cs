using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtility.Interface
{
    /// <summary>
    /// 平台作业
    /// </summary>
    interface IControlTask
    {
        /// <summary>
        /// 作业监控
        /// </summary>
        void TaskMonitor();
        /// <summary>
        /// 作业统计
        /// </summary>
        void TaskCollection();
    }
}
