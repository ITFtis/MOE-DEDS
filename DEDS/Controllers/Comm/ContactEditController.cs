using DEDS.Models;
using DEDS.Models.Comm;
using DEDS.Models.Manager;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "ContactEdit", Name = "通聯造冊管理", MenuPath = "通聯資料", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class ContactEditController : Dou.Controllers.AGenericModelController<Tabulation>
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();
        // GET: ContactEdit
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Tabulation> GetModelEntity()
        {
            return new ModelEntity<Tabulation>(new DouModelContextExt());
        }

        protected override IEnumerable<Tabulation> GetDataDBObject(IModelEntity<Tabulation> dbEntity, params KeyValueParams[] paras)
        {
            IEnumerable<Tabulation> blankresult = null;
            var filterCategory = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "CategoryId");

            if (filterCategory != null)
            {
                var BaseList = Db.UserBasic.ToList(); // 基本資料表
                ////var PositionList = fun.GetPosition();
                var iquery = base.GetDataDBObject(dbEntity, paras);
                string UserID = Dou.Context.CurrentUser<User>().Id;
                string PWD = Dou.Context.CurrentUser<User>().Password;
                bool IsManager = Dou.Context.CurrentUser<User>().IsManager;
                if (!IsManager)
                {
                    //List<LoginInfo> Info = fun.GetInfo(UserID, PWD);
                    string CityID = fun.GetInfo(UserID, PWD); // 取得登錄者的部門ID
                    iquery = iquery.Where(s => s.CityID == CityID).OrderBy(e => e.Act ? 0 : 1).ThenBy(w => w.Sort).ToList();
                    var result = new List<Tabulation>();
                    foreach (var item in iquery)
                    {
                        UserBasic UserBase = GetBase(BaseList, item.UID)[0];
                        result.Add(new Tabulation
                        {
                            No = item.No,
                            UID = item.UID,
                            Name = UserBase.Name,
                            CityID = UserBase.CityID,
                            CategoryId = item.CategoryId,
                            Sort = item.Sort,
                            Act = item.Act,
                            PositionId = item.PositionId,//fun.GetPositionName(PositionList, UserBase.PositionId),
                            OfficePhone = UserBase.OfficePhone,
                            MobilePhone = UserBase.MobilePhone,
                            Email = UserBase.Email,
                            Note = UserBase.Note,
                        });
                    }
                    return result;
                }
                else
                {
                    var result = new List<Tabulation>();
                    iquery = iquery.OrderBy(e => e.Act ? 0 : 1).ThenBy(w => w.Sort).ToList();
                    foreach (var item in iquery)
                    {
                        UserBasic UserBase = GetBase(BaseList, item.UID)[0];
                        result.Add(new Tabulation
                        {
                            No = item.No,
                            UID = item.UID,
                            Name = UserBase.Name,
                            CityID = UserBase.CityID,
                            CategoryId = item.CategoryId,
                            Sort = item.Sort,
                            Act = item.Act,
                            PositionId = item.PositionId,//fun.GetPositionName(PositionList, UserBase.PositionId),
                            OfficePhone = UserBase.OfficePhone,
                            MobilePhone = UserBase.MobilePhone,
                            Email = UserBase.Email,
                            Note = UserBase.Note,
                        });
                    }
                    return result;

                }

            }
            else
                blankresult = new List<Tabulation>(); //強迫要有查尋條件
            return blankresult;


        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.GetFiled("No").visible = false;

            return opts;
        }

        public List<UserBasic> GetBase(List<UserBasic> BaseList, string UID)
        {
            List<UserBasic> result = new List<UserBasic> { BaseList.Where(q => q.UID == UID).FirstOrDefault() };
            return result;
        }       

    }
}