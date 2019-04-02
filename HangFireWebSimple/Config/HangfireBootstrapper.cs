
/*===================================================
* 类名称: HangfireBootstrapper
* 类描述: HangFire保持自启动的配置类
* 创建人: 李先锋
* 创建时间: 2019/3/29 10:24:51
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace HangFireWebSimple.Config
{
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();

        private readonly object _lockObject = new object();
        private bool _started;

        private BackgroundJobServer _backgroundJobServer;

        private HangfireBootstrapper()
        {
        }
        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;
                _started = true;

                HostingEnvironment.RegisterObject(this);

                GlobalConfiguration.Configuration
                    .UseSqlServerStorage("Data Source=localhost;User Id=sa;Password=123456;Database=HangFireSimple;Pooling=true;Max Pool Size=5000;Min Pool Size=0;");
                // Specify other options here

                _backgroundJobServer = new BackgroundJobServer();
            }
        }
        public void Stop()
        {
            lock (_lockObject)
            {
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                }

                HostingEnvironment.UnregisterObject(this);
            }
        }
        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
    }
}