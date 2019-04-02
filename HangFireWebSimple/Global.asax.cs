using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Hangfire;
using HangFireWebSimple.Config;

namespace HangFireWebSimple
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //自启动配置
            HangfireBootstrapper.Instance.Start();
        }
        protected void Application_End(object sender, EventArgs e)
        {
            //自启动配置
            HangfireBootstrapper.Instance.Stop();
        }
    }
}