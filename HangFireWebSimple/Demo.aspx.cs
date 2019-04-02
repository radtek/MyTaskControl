﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hangfire;
using HangFireWebSimple.TaskGroup;
using NLog;
using HangFireWebSimple.Helper;

namespace HangFireWebSimple
{
    public partial class Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            int mathNum = 1996;
            int result = (int)AssemblyHelper.DLLFuncWithParam(out string expInfo, @"I:\API开发（Romens雨人）\LearnWay\HongFireSimple\Demo\bin\Debug\Demo.dll", "Demo", "Math", "Add", new Type[] {Type.GetType("System.Int32")}, mathNum);
            int result1 = (int)AssemblyHelper.DLLFunc(out string error, @"I:\API开发（Romens雨人）\LearnWay\HongFireSimple\Demo\bin\Debug\Demo.dll", "Demo", "Math", "Reduce");
            AssemblyHelper.DLLAction(out string e2, @"I:\API开发（Romens雨人）\LearnWay\HongFireSimple\Demo\bin\Debug\Demo.dll", "Demo", "Math", "Errormethod");
            int[] ints;
            
            TaskLog taskLog = new TaskLog();
            Task t = new Task(() => {
                taskLog.DebugLog();
            });
            //重复任务
            RecurringJob.AddOrUpdate("20190323LoopTask",() => taskLog.DebugLog(),Cron.MinuteInterval(5));
            
        }
    }
}