using DEDS.Models;
using DEDS.Models.Comm;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "Position", Name = "職稱代碼", MenuPath = "通聯資料", Action = "Index", Index = 5, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class PositionController : Dou.Controllers.AGenericModelController<Position>
    {
        // GET: Position
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Position> GetModelEntity()
        {
            return new ModelEntity<Position>(new DouModelContextExt());
        }
    }
}