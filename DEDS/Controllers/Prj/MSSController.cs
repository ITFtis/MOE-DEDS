using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Prj
{
    [HtmlIFrameMenuDef(Id = "Mss", Name = "EMIS", MenuPath = "災情決策", Index = 3, Url = "https://newemis.epa.gov.tw/Home/Login", IsPromptUI =true)]
    public class MSSController : Controller
    {
        // GET: MSS
        public ActionResult Index()
        {
            return View();
        }
    }
}