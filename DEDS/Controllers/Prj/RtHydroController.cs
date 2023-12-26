using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Prj
{
    [MenuDef(Id = "RtHydro", Name = "決策支援圖台", MenuPath = "災情決策", Index = 1, Action = "Index", AllowAnonymous = false, Func = FuncEnum.None)]
    public class RtHydroController : Dou.Controllers.AGenericModelController<Object>
    {
        // GET: RtHydro
        public ActionResult Index()
        {
            ViewBag.HasGis = true;
            ViewBag.Leaflet = true;
            ViewBag.ToolNearCctv = true;
            return View();
        }

        protected override IModelEntity<object> GetModelEntity()
        {
            throw new NotImplementedException();
        }
    }
}