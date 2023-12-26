using Microsoft.Owin;
using Owin;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(DEDS.Startup))]

namespace DEDS
{
    public class Startup
    {
        internal static AppSet AppSet { get; set; }
        public void Configuration(IAppBuilder app)
        {
            bool isDebug = true;
            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            Dou.Context.Init(new Dou.DouConfig
            {
                //SystemManagerDBConnectionName = "DouModelContextExt",
                DefaultPassword = "1234@1qaz#EDC", //"1234@1qaz#EDC",
                SessionTimeOut = 20,
                SqlDebugLog = isDebug,
                AllowAnonymous = false,
                LoginPage = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Action("DouLogin", "User", new { }),
                LoggerListen = (log) =>
                {
                    if (log.WorkItem == Dou.Misc.DouErrorHandler.ERROR_HANDLE_WORK_ITEM)
                    {
                        Debug.WriteLine("DouErrorHandler發出的錯誤!!\n" + log.LogContent);
                        Logger.Log.For(null).Error("DouErrorHandler發出的錯誤!!\n" + log.LogContent);
                    }
                },
                EnforceChangePasswordFirstLogin = false,
                VerifyUserPasswordErrorCount = 9999,
                RenewPasswordDeadline = int.MaxValue,
                VerifyUserErrorLockTime = 10,
                RenewPasswordInterval = 0,
                OnAModelControllerActionExecuting = (ctx) => {
                    return null;
                }
            });
            AppSet = DouHelper.Misc.DeSerializeObjectLoadJsonFile<AppSet>(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Config"), "AppSet.json"));
            var jf = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/Config")), "epa-moe-1695180571378-968ff831495b.json");
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jf);//環境變數
        }
    }
}
