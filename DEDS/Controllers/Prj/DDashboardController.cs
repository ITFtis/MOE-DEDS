using Dou.Misc.Attr;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Prj
{
    //[MenuDef(Id = "DDashboard", Name = "災情儀錶板", MenuPath = "災情決策", Index = 1, Action = "Index", AllowAnonymous = false, Func = FuncEnum.None)]
    public class DDashboardController : Dou.Controllers.AGenericModelController<Object>
    {
        // GET: DDashboard
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