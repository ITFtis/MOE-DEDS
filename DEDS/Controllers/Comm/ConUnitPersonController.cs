using DEDS.Models;
using DEDS.Models.Comm;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                    ViewBag.ConUnitName = v.Name;
                }
            }

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

            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            KeyValueParams korder = paras.FirstOrDefault((KeyValueParams s) => s.key == "order");
            //分頁排序
            if (ksort.value != null && korder.value != null)
            {
            }
            else
            {
                //預設排序                
                query = query.OrderBy(a => a.ConUnitSort).ThenBy(a => a.PSort);
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

            return opts;
        }

        public ActionResult UpdateConfirm(List<int> Ids)
        {
            try
            {
                //確認日期更新
                var f = GetModelEntity();
                var iquery = f.GetAll().Where(a => Ids.Any(b => b == a.Id));
                int n = iquery.Count();
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
    }
}