using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using DouHelper;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        public virtual ActionResult SaveMailJson(IEnumerable<TestMailParam> objs)
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

                throw new Exception("更新Mail設定值(json)失敗：" + ex.Message);
            }

            return Json(new { Success = success, Desc = desc, data = f }, JsonRequestBehavior.AllowGet);
        }
    }
}