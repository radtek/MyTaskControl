/*===================================================
* 类名称: HangfireBootstrapper
* 类描述: HangFire保持自启动的配置类
* 创建人: 李先锋
* 创建时间: 2019/3/29 10:24:51
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace HangFireWebSimple.Config
{
    public class ApplicationPreload : System.Web.Hosting.IProcessHostPreloadClient
    {
        void IProcessHostPreloadClient.Preload(string[] parameters)
        {
            HangfireBootstrapper.Instance.Start();
        }
    }
}