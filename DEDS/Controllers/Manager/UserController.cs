﻿using DEDS.Models;
using DEDS.Models.Comm;
using DEDS.Models.Epaemis_local;
using DEDS.Models.Manager;
using Dou.Misc;
using Dou.Models;
using Google.Api.Gax.ResourceNames;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Google.Rpc.Context.AttributeContext.Types;
using static NPOI.HSSF.Util.HSSFColor;

namespace DEDS.Controllers.Manager
{
    [Dou.Misc.Attr.MenuDef(Id = "User", Name = "使用者管理", MenuPath = "系統管理", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class UserController : Dou.Controllers.UserBaseControll<User, Role>
    {
        public DouModelContextExt Db = new DouModelContextExt();
        string recaptchaProjectID = null;
        string recaptchaSiteKey = null;
        string recaptchaAction = null;
        public UserController()
        {
            recaptchaProjectID = System.Configuration.ConfigurationManager.AppSettings["reCAPTCHA-projectID"];
            recaptchaSiteKey = System.Configuration.ConfigurationManager.AppSettings["reCAPTCHA-siteKey"];
            recaptchaAction = System.Configuration.ConfigurationManager.AppSettings["reCAPTCHA-Action-login"];
            //環境變數改至Startup做
            //var jf = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/Config")), "epa-moe-1695180571378-968ff831495b.json");
            //System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jf);//環境變數
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        protected override Dou.Models.DB.IModelEntity<User> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<User>(new DouModelContextExt());
        }
        public override ActionResult DouLogin(User user, string returnUrl, bool redirectLogin = false)
        {
            if (user.Id == null)
            {
                //新增使用者，再一次DouLogin
                var newUser = TempData["newUser"];
                if (newUser != null)
                {
                    user = (User)newUser;
                }
            }

            var vuser = TempData["vuser"] as User;
            if (vuser != null)
            {
               return base.DouLogin(vuser, returnUrl, redirectLogin);
            }

            ViewBag.recaptchaSiteKey = recaptchaSiteKey;
            ViewBag.recaptchaAction = recaptchaAction;
            var token = Request.Params["token"];
            if (IsPostBack())
            {
                bool ischeck = true;
                string err = "";
                try
                {
                    ////暫時關閉recaptcha驗證 正祥：20240220
                    ////if (String.IsNullOrWhiteSpace(token))
                    ////{
                    ////    ischeck = false;
                    ////}
                    //else ////這邊在會內效能很慢，第一次IIS啟動後第一次驗證要約90秒
                    //    ischeck = createAssessmentAsync(token);
                }
                catch (Exception e)
                {
                    err = e.Message;
                    ischeck = false;
                }
                if (!ischeck)
                {
                    ViewBag.ErrorMessage = err == null ? "未通過reCAPTCHA 保護機制" : err;
                    //return View(user); 20210319
                    return PartialView(user);
                }

                if (!string.IsNullOrEmpty(user.Id) && !string.IsNullOrEmpty(user.Password))
                {
                    var u = FindUser(user.Id); //先搜尋資料庫內的user
                    if (u == null && Dou.Help.DouUnobtrusiveSession.Session[OldSysUserLoginKey] != null) //找不到使用者 但是是從舊系統來的
                    {
                        //to do Api驗證
                        //var IsOldSysUser = CheckOldSysUser(user.Id, user.Password);

                        Dou.Models.DB.IModelEntity<Models.Epaemis_local.Users> vm = new Dou.Models.DB.ModelEntity<Models.Epaemis_local.Users>(new EpaemisContextExt());
                        var OldsysInfo = vm.GetAll().Where(a => a.UserName == user.Id && a.Pwd == user.Password).FirstOrDefault();

                        if (OldsysInfo != null)
                        {
                            u = new User()
                            {
                                Id = user.Id,
                                Name = OldsysInfo.Name,
                                Password = Dou.Context.Config.PasswordEncode(user.Password.Trim()),
                                DefaultPage = "RtHydro", //決策支援圖台
                                Enabled = true,
                                IsManager = false,
                                Unit = OldsysInfo.CityId == null ? "" : OldsysInfo.CityId.ToString(),
                                Mobile = OldsysInfo.MobilePhone == null ? "" : OldsysInfo.MobilePhone.ToString(),
                                Tel = OldsysInfo.OfficePhone == null ? "" : OldsysInfo.OfficePhone.ToString(),
                                EMail = OldsysInfo.Email == null ? "" : OldsysInfo.Email.ToString(),
                                Organize = OldsysInfo.Duty == null ? "" : OldsysInfo.Duty.ToString(),
                                //RoleUsers = new RoleUser[] { new RoleUser { RoleId = defaultRoleId, UserId = user.Id } }.ToList()
                            };
                            //配置預設角色(role)
                            string roleId = "user2";
                            u.RoleUsers = new RoleUser[] { new RoleUser { RoleId = roleId, UserId = user.Id } }.ToList();

                            this.AddDBObject(GetModelEntity(), new User[] { u });

                            if (Dou.Context.Config.VerifyPassword(u.Password, user.Password.Trim()))
                            {
                                u.Password = Dou.Context.Config.PasswordEncode(user.Password.Trim());
                                this.UpdateDBObject(GetModelEntity(), new User[] { u });
                            }

                            //重新登入驗證                            
                            TempData["newUser"] = u;
                            return RedirectToAction("DouLogin", new { returnUrl = returnUrl, redirectLogin = redirectLogin });
                        }
                        else
                        {
                            ViewBag.ErrorMessage = string.Format("(Emis)查無此帳密資料：{0}({1})", user.Id, user.Password);
                            return PartialView(user);
                        }
                    }
                    else if (u != null && Dou.Context.Config.VerifyPassword(u.Password, user.Password.Trim())) // 驗證成功就進入系統
                    {
                        u.Password = Dou.Context.Config.PasswordEncode(user.Password.Trim());
                        this.UpdateDBObject(GetModelEntity(), new User[] { u });
                    }
                    else //驗證失敗
                    {
                        ViewBag.ErrorMessage = "帳號密碼有誤！";
                        return PartialView(user);
                    }
                }
            }

            //更換帳號清cache
            ConUnitCodeItems.Reset();

            if (user.Id != null)
            {
                return LoginRedirect(user, returnUrl, redirectLogin);
            }
            else
            {
                return base.DouLogin(user, returnUrl, redirectLogin);
            }
        }

        public ActionResult LoginRedirect(Models.Manager.User user, string returnUrl, bool redirectLogin)
        {
            if (IsSystemNotice())
            {
                TempData["vuser"] = user;
                return View("~/Views/User/SystemNotice.cshtml", user);
            }

            return base.DouLogin(user, returnUrl, redirectLogin);
        }

        /// <summary>
        /// 系統公告訊息
        /// </summary>
        /// <returns></returns>
        public bool IsSystemNotice()
        {
            bool result = false;

            DateTime date = DateTime.Now;

            DateTime sDate1 = DateTime.Parse("2024/10/19 00:00");
            DateTime eDate1 = DateTime.Parse("2024/10/21 22:30");

            if (date >= sDate1 && date <= eDate1)
            {
                return true;
            }

            ////DateTime sDate2 = DateTime.Parse("2024/08/22 00:00");
            ////DateTime eDate2 = DateTime.Parse("2024/08/25 22:00");

            ////if (date >= sDate2 && date <= eDate2)
            ////{
            ////    return true;
            ////}

            return result;
        }

        internal string OldSysUserLoginKey = "~OldSysUserLoginKey~";
        [AllowAnonymous]
        public ActionResult Login()
        {
            Dou.Help.DouUnobtrusiveSession.Session.Add(OldSysUserLoginKey, true);
            return Redirect(Dou.Context.Config.LoginPage);
        }
        // Create an assessment to analyze the risk of an UI action.
        // projectID: GCloud Project ID.
        // recaptchaSiteKey: Site key obtained by registering a domain/app to use recaptcha.
        // token: The token obtained from the client on passing the recaptchaSiteKey.
        // recaptchaAction: Action name corresponding to the token.
        private bool createAssessmentAsync(string token
            //,
            //string projectID = "epa-moe-1695180571378",
            //string recaptchaSiteKey = "6Ld75DsoAAAAADoRyOziKXJg_zfZnFxLLqs5goo2",
            //string recaptchaAction = "LOGIN"
            )
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                throw new Exception("The `token` argument must not empty.");
            }

            ProjectName projectName = new ProjectName(recaptchaProjectID);

            // Build the assessment request.
            CreateAssessmentRequest createAssessmentRequest = new CreateAssessmentRequest()
            {
                Assessment = new Assessment()
                {
                    // Set the properties of the event to be tracked.
                    Event = new Event()
                    {
                        SiteKey = recaptchaSiteKey,
                        Token = token,
                        ExpectedAction = recaptchaAction
                    },
                },
                ParentAsProjectName = projectName
            };
            //json取得，參考 https://blog.miniasp.com/post/2023/01/15/How-to-Integrate-reCAPTCHA-Enterprise-with-ASPNET-Core


            //RecaptchaEnterpriseServiceClient.Create()前須先設GOOGLE_APPLICATION_CREDENTIALS環境變數
            RecaptchaEnterpriseServiceClient _recaptchaClient = RecaptchaEnterpriseServiceClient.Create();
            DateTime st = DateTime.Now;
            Assessment response = _recaptchaClient.CreateAssessment(createAssessmentRequest); //這邊在會內效能很慢，第一次IIS啟動後第一次驗證要約90秒
            Logger.Log.For(this).Info($"recaptchaClient.CreateAssessment elapsed time {(DateTime.Now - st).TotalMilliseconds}");

            // Check if the token is valid.
            if (response.TokenProperties.Valid == false)
            {
                throw new Exception("The CreateAssessment call failed because the token was: " +
                    response.TokenProperties.InvalidReason.ToString());

            }

            // Check if the expected action was executed.
            if (response.TokenProperties.Action != recaptchaAction)
            {
                throw new Exception("The action attribute in reCAPTCHA tag is: " +
                    response.TokenProperties.Action.ToString() + "<br>" +
                    "The action attribute in the reCAPTCHA tag does not " +
                    "match the action you are expecting to score");
            }

            // Get the risk score and the reason(s).
            // For more information on interpreting the assessment,
            // see: https://cloud.google.com/recaptcha-enterprise/docs/interpret-assessment

            var score = response.RiskAnalysis.Score;

            Logger.Log.For(this).Info("The reCAPTCHA score is: " + score);

            foreach (RiskAnalysis.Types.ClassificationReason reason in response.RiskAnalysis.Reasons)
            {
                Logger.Log.For(this).Info(reason.ToString());
            }

            // https://developers.google.com/recaptcha/docs/analytics
            // This chart shows the average score on your site, which is designed to help you spot trends.
            // Scores range from 0.0 to 1.0, with 0.0 indicating abusive traffic and 1.0 indicating good traffic.
            // Sign up for reCAPTCHA v3 to gain more insights about your traffic.
            if (score > 0.5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public KeyValuePair<bool, string> CheckOldSysUser(string UserID, string PWD)
        {
            var options = new RestClientOptions(Startup.AppSet.OldSysUserLoginApi)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(Startup.AppSet.OldSysUserLoginApi, Method.Post);
            request.AddHeader("token", "aS6bQK2Bj4rS[awqFY&"); //預防不是從系統Post的路徑呼叫的
            request.AddHeader("Content-Type", "application/json");
            var body = @"{""UserName"":""" + UserID + @""",""Pwd"":""" + PWD + @"""}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = client.Execute(request);
            var jsonstring = response.Content;
            try
            {
                var Result = JsonConvert.DeserializeObject<List<LoginInfo>>(jsonstring);
                return new KeyValuePair<bool, string>(true, jsonstring);
            }
            catch //驗證失敗 不管是帳號資料異常或是登入失敗都是
            {

                return new KeyValuePair<bool, string>(false, jsonstring);
            }
        }

        public class LoginInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int CityId { get; set; }
            public object MobilePhone { get; set; }
            public string OfficePhone { get; set; }
            public string Email { get; set; }
            public string Duty { get; set; }
        }
    }
}