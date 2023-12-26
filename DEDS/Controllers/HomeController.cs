using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers
{
    //[Dou.Misc.Attr.MenuDef(Id = "Home", Name = "首頁", MenuPath = "", Action = "Index", Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = true)]
    public class HomeController : Dou.Controllers.AGenericModelController<Object>
    {
        // GET: Home
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