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
    [Dou.Misc.Attr.MenuDef(Id = "ConUnitCode", Name = "應變單位代碼", MenuPath = "幕僚/窗口", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ConUnitCodeController : Dou.Controllers.AGenericModelController<ConUnitCode>
    {
        // GET: ConUnitCode
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<ConUnitCode> GetModelEntity()
        {
            return new ModelEntity<ConUnitCode>(new DouModelContextExt());
        }

        protected override IEnumerable<ConUnitCode> GetDataDBObject(IModelEntity<ConUnitCode> dbEntity, params KeyValueParams[] paras)
        {
            var query = base.GetDataDBObject(dbEntity, paras);

            KeyValueParams ksort = paras.FirstOrDefault((KeyValueParams s) => s.key == "sort");
            KeyValueParams korder = paras.FirstOrDefault((KeyValueParams s) => s.key == "order");
            //分頁排序
            if (ksort.value != null && korder.value != null)
            {
            }
            else
            {
                //預設排序                
                query = query.OrderBy(a => a.Sort);
            }

            return query;
        }

        protected override void AddDBObject(IModelEntity<ConUnitCode> dbEntity, IEnumerable<ConUnitCode> objs)
        {
            base.AddDBObject(dbEntity, objs);
            ConUnitCodeItems.Reset();
        }

        protected override void UpdateDBObject(IModelEntity<ConUnitCode> dbEntity, IEnumerable<ConUnitCode> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            ConUnitCodeItems.Reset();
        }

        protected override void DeleteDBObject(IModelEntity<ConUnitCode> dbEntity, IEnumerable<ConUnitCode> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            ConUnitCodeItems.Reset();
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            return opts;
        }
    }
}