using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HangFireWebSimple.Startup))]

namespace HangFireWebSimple
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=localhost;User Id=sa;Password=123456;Database=HangFireSimple;Pooling=true;Max Pool Size=5000;Min Pool Size=0;");
            var filter = new BasicAuthAuthorizationFilter(
              new BasicAuthAuthorizationFilterOptions
              {
                  //SSL不启用
                  SslRedirect = false,
                  //需要仪表板的安全连接SSL
                  RequireSsl = false,
                  //区分大小写的登录检查
                  LoginCaseSensitive = false,
                  Users = new[]

                  {
                        new BasicAuthAuthorizationUser
                        //最高管理员
                        {

                            Login = "FF",//用户名
                            PasswordClear = "afandxx"//密码
                        },
                        //开发人员统一账号
                        new BasicAuthAuthorizationUser
                        {

                            Login = "Developer",//用户名
                            PasswordClear = "romens"//密码
                        },
                        //实施人员统一账号
                        new BasicAuthAuthorizationUser
                        {
                            Login = "executant",//用户名
                            PasswordClear = "romens"//密码
                        },
                  }
              });
            //修改映射URL
            app.UseHangfireDashboard("/TaskQueue/Manager",new DashboardOptions() { AuthorizationFilters = new[] { filter } });
            app.UseHangfireServer();
            
        }
    }
}
