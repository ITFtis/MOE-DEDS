using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Path
{
    //[Dou.Misc.Attr.MenuDef(Name = "系統管理", Index = int.MaxValue, IsOnlyPath = true, Icon = "~/images/menu/menu_admin.png")]
    public class ManagerPathController : Controller
    {
        // GET: ManagerPath
        public ActionResult Index()
        {
            return View();
        }
    }
}