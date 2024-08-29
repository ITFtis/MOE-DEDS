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
using static Google.Cloud.RecaptchaEnterprise.V1.AccountVerificationInfo.Types;

namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "ContactEdit", Name = "通聯造冊管理", MenuPath = "緊急應變通聯手冊", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
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

            Dou.Help.DouUnobtrusiveSession.Session.Remove("KeyTabulationList");

            if (filterCategory != null)
            {
                var BaseList = Db.UserBasic.ToList(); // 基本資料表
                ////var PositionList = fun.GetPosition();
                var iquery = base.GetDataDBObject(dbEntity, paras);
                string UserID = Dou.Context.CurrentUser<User>().Id;
                string PWD = Dou.Context.CurrentUser<User>().Password;
                bool IsManager = Dou.Context.CurrentUser<User>().IsManager;

                var result = new List<Tabulation>();
                if (!IsManager)
                {
                    //List<LoginInfo> Info = fun.GetInfo(UserID, PWD);
                    string CityID = fun.GetInfo(UserID, PWD); // 取得登錄者的部門ID
                    iquery = iquery.Where(s => s.CityID == CityID).OrderBy(e => e.Act ? 0 : 1).ThenBy(w => w.Sort).ToList();
                    
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
                            PositionId = UserBase.PositionId,//fun.GetPositionName(PositionList, UserBase.PositionId),
                            OfficePhone = UserBase.OfficePhone,
                            MobilePhone = UserBase.MobilePhone,
                            Email = UserBase.Email,
                            Note = UserBase.Note,
                            ConfirmDate = item.ConfirmDate,
                        });
                    }
                }
                else
                {                    
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
                            PositionId = UserBase.PositionId,//fun.GetPositionName(PositionList, UserBase.PositionId),
                            OfficePhone = UserBase.OfficePhone,
                            MobilePhone = UserBase.MobilePhone,
                            Email = UserBase.Email,
                            Note = UserBase.Note,
                            ConfirmDate = item.ConfirmDate,
                        });
                    }                                                            
                }

                Dou.Help.DouUnobtrusiveSession.Session.Add("KeyTabulationList", result);
                return result;
            }
            else
                blankresult = new List<Tabulation>(); //強迫要有查尋條件
            return blankresult;


        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.GetFiled("No").visible = false;
            opts.GetFiled("StrConfirmDate").visible = true;

            return opts;
        }

        public List<UserBasic> GetBase(List<UserBasic> BaseList, string UID)
        {
            List<UserBasic> result = new List<UserBasic> { BaseList.Where(q => q.UID == UID).FirstOrDefault() };
            return result;
        }

        public ActionResult UpdateConfirm(List<int> Ids)
        {
            try
            {
                var KeyTabulationList = Dou.Help.DouUnobtrusiveSession.Session["KeyTabulationList"];
                if (KeyTabulationList == null)
                {
                    return Json(new { result = false, errorMessage = "清單無資料" });
                }

                List<Tabulation> list = (List<Tabulation>)KeyTabulationList;
                List<int> nos = list.Where(a => a.Act).Select(a => a.No).ToList();

                //確認日期更新
                var model = GetModelEntity();
                var iquery = model.GetAll().Where(a => nos.Any(b => b == a.No))                                        ;
                iquery.ToList().ForEach(p => p.ConfirmDate = DateTime.Now);

                model.Update(iquery);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, errorMessage = ex.Message });
            }
        }
    }
}