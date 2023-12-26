using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Path
{
    //[Dou.Misc.Attr.MenuDef(Name = "災情決策", Index = 1, IsOnlyPath = true, Icon = "~/images/menu/menu_system.png")]
    public class PrjPathController : Controller
    {
        // GET: Prj
        public ActionResult Index()
        {
            return View();
        }
    }
}