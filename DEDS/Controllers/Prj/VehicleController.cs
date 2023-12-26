using Antlr.Runtime.Misc;
using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DEDS.Controllers.Prj
{
    [MenuDef(Id = "CleanVehicle", Name = "清潔車輛", MenuPath = "倉儲管理", Index = 1, AllowAnonymous =false)]
    public class VehicleController : Dou.Controllers.AGenericModelController<object>
    {
        // GET: Disinfector
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<object> GetModelEntity()
        {
            throw new NotImplementedException();
        }
    }
}