using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers
{
    public class HtmlIframeOtherMainLayoutController :Dou.Controllers.HtmlIFrameController
    {
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            ctx.Result = View("HtmlIFramePartial", "OtherManLayout");
        }
    }
}