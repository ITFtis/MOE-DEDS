using DEDS.Models.Manager;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using DouHelper;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace DEDS.Controllers.Manager
{
    [Dou.Misc.Attr.MenuDef(Id = "TestMail", Name = "Mail設定", MenuPath = "系統管理", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class TestMailController : APaginationModelController<TestMailParam>
    {
        // GET: TestMail
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<TestMailParam> GetModelEntity()
        {
            throw new NotImplementedException();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.addable = false;
            //opts.editable = false;
            opts.deleteable = false;

            opts.singleDataEdit = true;

            TestMailParam data = new TestMailParam();
            data.iniParam();
            opts.datas = new List<TestMailParam>() { data };

            opts.editformWindowStyle = "showEditformOnly";

            return opts;
        }

        protected override void UpdateDBObject(IModelEntity<TestMailParam> dbEntity, IEnumerable<TestMailParam> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
        }

        public virtual ActionResult ToSendMail(IEnumerable<TestMailParam> objs)
        {
            var f = objs.FirstOrDefault();

            if (string.IsNullOrEmpty(f.Account))
                return Json(new { Success = false, Desc = "寄發失敗，帳號參數錯誤", data = f }, JsonRequestBehavior.AllowGet);

            bool success = false;
            string desc = "";

            try
            {
                //(1)設定內容 (特休結算通知)
                string body = "";
                string path = AppConfig.HtmlTemplatePath + "0_測試Mail.html";
                using (StreamReader reader = System.IO.File.OpenText(path))
                {
                    body = reader.ReadToEnd();
                }

                if (string.IsNullOrEmpty(body))
                {
                    Logger.Log.For(null).Error("ToSend - html body取出無內容：" + path);
                    return Json(new { Success = success, Desc = "html body取出無內容", data = f }, JsonRequestBehavior.AllowGet);
                }

                body = body.Replace("[DateNow]", DateFormat.ToDate4(DateTime.Now))
                        .Replace("[LoginUser]", Dou.Context.CurrentUser<User>().Name);


                //(2)設定Helper
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.MailFrom = f.MailFrom;
                emailHelper.MailFromName = f.MailFromName;
                emailHelper.Account = f.Account;
                emailHelper.Password = f.Password;
                emailHelper.MailServer = f.MailServer;
                emailHelper.MailPort = f.MailPort;
                emailHelper.EnableSSL = f.EnableSSL;

                string subject = "測試信件(TestMail)";
                emailHelper.Subject = subject;
                emailHelper.Body = body;

                foreach (string addr in f.ToMails.Split(','))
                {
                    if (addr != "")
                    {
                        emailHelper.AddTo(addr, "");
                    }
                }

                foreach (string addr in f.BCCMails.Split(','))
                {
                    if (addr != "")
                    {
                        emailHelper.AddBCC(addr, "");
                    }
                }

                emailHelper.IsSendEmail = true;
                success = emailHelper.SendBySmtp();
                
                if (!success)
                {
                    desc = "信件寄發失敗";
                    Logger.Log.For(null).Error("ToSend - 信件寄發失敗:" + emailHelper.ToMails);
                }
                else
                {
                    desc = "信件已寄出";
                }
            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error("更新Mail設定值(json)失敗：" + ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);

                desc = "信件寄發錯誤：" + ex.Message;
            }

            return Json(new { Success = success, Desc = desc, data = f }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult ToSaveMailJson(IEnumerable<TestMailParam> objs)
        {
            var f = objs.FirstOrDefault();

            if (string.IsNullOrEmpty(f.Account))
                return Json(new { Success = false, Desc = "更新失敗，帳號參數錯誤", data = f }, JsonRequestBehavior.AllowGet);

            bool success = false;
            string desc = "";

            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Data/TestMailParam.json");
                using (var streamWriter = System.IO.File.CreateText(path))
                {
                    var text = new JavaScriptSerializer().Serialize(f);
                    streamWriter.Write(text);

                    success = true;
                    desc = "更新成功";
                }
            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error("更新Mail設定值(json)失敗：" + ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);

                desc = "更新失敗";
            }

            return Json(new { Success = success, Desc = desc, data = f }, JsonRequestBehavior.AllowGet);
        }
    }
}