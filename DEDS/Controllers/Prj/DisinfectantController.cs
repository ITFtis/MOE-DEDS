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
    [MenuDef(Id = "Disinfectant", Name = "消毒藥劑", MenuPath = "倉儲管理", Index = 5, AllowAnonymous = false)]
    public class DisinfectantController :Dou.Controllers.AGenericModelController<object>
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