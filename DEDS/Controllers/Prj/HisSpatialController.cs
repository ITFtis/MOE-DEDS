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
    [MenuDef(Id = "HisSpatial", Name = "歷史空間資料", MenuPath = "災情決策", Index = 5, Action = "Index",  Func = FuncEnum.None)]
    public class HisSpatialController :Controller//  Dou.Controllers.AGenericModelController<object>
    {
        // GET: HisSpatial
        public ActionResult Index()
        {
            return View();
        }

        //protected override IModelEntity<object> GetModelEntity()
        //{
        //    throw new NotImplementedException();
        //}
    }
}