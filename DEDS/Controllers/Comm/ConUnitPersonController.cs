using Antlr.Runtime.Misc;
using DEDS.Models;
using DEDS.Models.Comm;
using DEDS.Models.Manager;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Google.Cloud.RecaptchaEnterprise.V1.TransactionData.Types;

namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "ConUnitPerson", Name = "應變人員清冊", MenuPath = "幕僚/窗口", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ConUnitPersonController : Dou.Controllers.AGenericModelController<ConUnitPerson>
    {
        // GET: ConUnitPerson
        public ActionResult Index()
        {
            var user = Dou.Context.CurrentUser<DEDS.Models.Manager.User>();

            //環境部(23)檢視所有單位資料，但只能修改自己
            string unit = Dou.Context.CurrentUser<DEDS.Models.Manager.User>().Unit;            
            if (unit == "23")
            {
                //全部
                ViewBag.IsView = false;
            }
            else
            {
                ViewBag.IsView = true;
            }

            //admin最大權限
            if (!user.IsManager)
            {
                string conUnit = user.ConUnit;
                var v = DEDS.Models.Comm.ConUnitCodeItems.ConUnitCodes.Where(a => a.Code == conUnit).FirstOrDefault();
                if (v != null)
                {
                    ViewBag.LoginConUnitName = v.Name;
                }
                else
                {
                    //User沒設定應變單位，預設xx，view控制不顯示編輯功能
                    ViewBag.LoginConUnitName = "xx";
                }

                //IsOrgStaff 幕僚人員                
                ViewBag.IsOrgStaff = IsOrgStaffByName(user.Name);
            }

            ViewBag.LoginIsManager = user.IsManager;
            ViewBag.LoginConUnit = user.ConUnit;

            return View();
        }

        protected override IModelEntity<ConUnitPerson> GetModelEntity()
        {
            return new ModelEntity<ConUnitPerson>(new DouModelContextExt());
        }

        protected override IEnumerable<ConUnitPerson> GetDataDBObject(IModelEntity<ConUnitPerson> dbEntity, params KeyValueParams[] paras)
        {
            var query = base.GetDataDBObject(dbEntity, paras);

            //預設條件
            query = query.Where(a => ConUnitCodeItems.ConUnitCodes.Any(b => b.Code == a.ConUnit));

            var user = Dou.Context.CurrentUser<DEDS.Models.Manager.User>();
            bool IsOrgCity = user.Unit != null && user.Unit != "23";     //環境部(23)
            if (IsOrgCity)
            {
                query = query.Where(a => a.ConType == 1);
            }

            ////if (IsOrgCity)
            ////{
            ////    //應變單位(縣市)：該縣市+聯繫窗口，但只能修改自己(query)
            ////    query = query.Where(a => ConUnitCodeItems.ConUnitCodes.Any(b => b.Code == a.ConUnit));

            ////    //檢視可看聯絡窗口(q2)
            ////    var q2 = base.GetDataDBObject(dbEntity, paras);
            ////    q2 = q2.Where(a => a.ConType == 1);

            ////    var ConUnitCodes = ConUnitCode.GetAllDatas();
            ////    Function fun = new Function();
            ////    var citys = fun.GetUnit();
            ////    foreach (var v in q2)
            ////    {
            ////        var f = ConUnitCodes.Where(a => a.Code == v.ConUnit).FirstOrDefault();
            ////        if (f != null)
            ////        {
            ////            var c = citys.Where(a => a.CityId == user.Unit).FirstOrDefault();
            ////            //特殊處理，(應變單位)下拉有權限設計，有符合對應不轉換中文。
            ////            if (c != null && c.Sector != f.Name)
            ////            {
            ////                v.ConUnit = f.Name;
            ////            }
            ////        }
            ////    }

            ////    query = query.Concat(q2).Distinct();                
            ////}
            ////else
            ////{
            ////    query = query.Where(a => ConUnitCodeItems.ConUnitCodes.Any(b => b.Code == a.ConUnit));
            ////}

            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            KeyValueParams korder = paras.FirstOrDefault((KeyValueParams s) => s.key == "order");
            //分頁排序
            if (ksort.value != null && korder.value != null)
            {
            }
            else
            {
                //預設排序(備註：應變單位(縣市)，因特殊處理造成ConUnit變中文，目前使用可接受)                
                query = query.OrderBy(a => a.EditSort)
                            .ThenBy(a => a.ConUnitSort).ThenBy(a => a.PSort);                
            }

            return query;
        }

        protected override void AddDBObject(IModelEntity<ConUnitPerson> dbEntity, IEnumerable<ConUnitPerson> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BId = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
        }

        protected override void UpdateDBObject(IModelEntity<ConUnitPerson> dbEntity, IEnumerable<ConUnitPerson> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UId = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
            {
                field.sortable = true;
                field.align = "left";
            }

            //IsOrgStaff 幕僚人員
            var user = Dou.Context.CurrentUser<DEDS.Models.Manager.User>();
            bool isOrgStaff = IsOrgStaffByName(user.Name);
            if (isOrgStaff)
            {
                opts.addable = false;
            }

            opts.GetFiled("StrConfirmDate").visible = true;

            return opts;
        }

        public ActionResult UpdateConfirm(List<int> Ids)
        {
            try
            {
                if (Ids == null)
                    return Json(new { result = false, errorMessage = "Ids：不可為Null" });

                //確認日期更新
                var f = GetModelEntity();
                var iquery = f.GetAll().Where(a => Ids.Any(b => b == a.Id));
                
                ////iquery = iquery.Take(47); /////////////////
                ////int n = iquery.Count(); /////////////////
                ////var test = iquery.ToList();

                if (iquery.Count() > 0)
                {
                    DateTime now = DateTime.Now;
                    foreach (var i in iquery)
                    {
                        i.ConfirmDate = now;
                    }

                    f.Update(iquery);

                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false, errorMessage = "查無對應Id：" + string.Join(",", Ids) });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = false, errorMessage = ex.Message });
            }
        }

        private bool IsOrgStaffByName(string name) {

            bool result = false;

            //幕僚人員，若同時聯繫窗口與幕僚人員(此非幕僚人員)
            var models = GetModelEntity().GetAll();
            var names = models.Where(a => a.ConType == 1).Select(a => a.Name);

            result = models.Where(a => !names.Any(b =>b == a.Name))
                        .Any(a => a.ConType == 2 && a.Name == name);

            return result;
        }
    }
}