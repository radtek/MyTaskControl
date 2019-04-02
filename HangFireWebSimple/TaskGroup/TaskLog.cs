using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace HangFireWebSimple.TaskGroup
{
    public class TaskLog
    {
        Logger nlog = LogManager.GetCurrentClassLogger();
        public void DebugLog()
        {
            nlog.Debug($"用户：{Environment.UserName}:is a big men!");
        }
    }
}