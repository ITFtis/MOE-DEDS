﻿using DEDS.Models;
using DEDS.Models.Comm;
using Dou.Controllers;
using Dou.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "ConUnitPerson", Name = "應變人員清冊", MenuPath = "緊急應變", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class ConUnitPersonController : Dou.Controllers.AGenericModelController<ConUnitPerson>
    {
        // GET: ConUnitPerson
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<ConUnitPerson> GetModelEntity()
        {
            return new ModelEntity<ConUnitPerson>(new DouModelContextExt());
        }

        protected override IEnumerable<ConUnitPerson> GetDataDBObject(IModelEntity<ConUnitPerson> dbEntity, params KeyValueParams[] paras)
        {
            var query = base.GetDataDBObject(dbEntity, paras);

            //預設條件
            query = query.Where(a => ConUnitCodeItems.ConUnitCodes.Any(b => b.Code == a.ConUnit));

            return query;
        }

        protected override void AddDBObject(IModelEntity<ConUnitPerson> dbEntity, IEnumerable<ConUnitPerson> objs)
        {
            var f = objs.First();

            f.BDate = DateTime.Now;
            f.BId = Dou.Context.CurrentUserBase.Id;
            f.BName = Dou.Context.CurrentUserBase.Name;

            base.AddDBObject(dbEntity, objs);
        }

        protected override void UpdateDBObject(IModelEntity<ConUnitPerson> dbEntity, IEnumerable<ConUnitPerson> objs)
        {
            var f = objs.First();

            f.UDate = DateTime.Now;
            f.UId = Dou.Context.CurrentUserBase.Id;
            f.UName = Dou.Context.CurrentUserBase.Name;

            base.UpdateDBObject(dbEntity, objs);
        }

        public ActionResult UpdateConfirm(List<int> Ids)
        {
            try
            {
                //確認日期更新
                var f = GetModelEntity();
                var iquery = f.GetAll().Where(a => Ids.Any(b => b == a.Id));
                if (iquery.Count() > 0)
                {
                    DateTime now = DateTime.Now;
                    foreach (var i in iquery)
                    {
                        i.ConfirmDate = now;
                    }
                }
                f.Update(iquery);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, errorMessage = ex.Message });
            }
        }
    }
}