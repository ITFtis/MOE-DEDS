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
using System.Web.Mvc;


namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "CommEdit", Name = "人員資料管理", MenuPath = "緊急應變通聯手冊", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class CommEditController : Dou.Controllers.AGenericModelController<UserBasic>
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();

        // GET: CommEdit
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<UserBasic> GetModelEntity()
        {
            return new ModelEntity<UserBasic>(new DouModelContextExt());
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            bool IsManager = Dou.Context.CurrentUser<User>().IsManager;
            if (!IsManager) { opts.GetFiled("CityID").visibleEdit = false; }
            else { opts.GetFiled("CityID").visible = true; }
                        
            return opts;
        }

        protected override IEnumerable<UserBasic> GetDataDBObject(IModelEntity<UserBasic> dbEntity, params KeyValueParams[] paras)
        {
            IEnumerable<UserBasic> blankresult = null;
            var filterCategory = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "CityID");
            var filterName = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "Name");

            if (filterCategory != null || filterName != null)
            {
                var iquery = base.GetDataDBObject(dbEntity, paras);
                string UserID = Dou.Context.CurrentUser<User>().Id;
                string PWD = Dou.Context.CurrentUser<User>().Password;
                bool IsManager = Dou.Context.CurrentUser<User>().IsManager;
                if (!IsManager)
                {
                    //List<LoginInfo> Info = fun.GetInfo(UserID, PWD);
                    string CityID = fun.GetInfo(UserID, PWD); // 取得登錄者的部門ID
                    var result = iquery.Where(s => s.CityID == CityID).ToList();
                    foreach (var item in result)
                    {
                        if (item.OfficePhone.Split('#')[1] == "")
                        {
                            item.OfficePhone = item.OfficePhone.Split('#')[0];
                        }
                    }
                    return result;
                }
                else
                {
                    iquery = iquery.OrderByDescending(q => q.CityID).ToList();
                    foreach (var item in iquery)
                    {
                        if (item.OfficePhone.Split('#')[1] == "")
                        {
                            item.OfficePhone = item.OfficePhone.Split('#')[0];
                        }
                    }
                    return iquery;
                }
            }
            else
            {
                blankresult = new List<UserBasic>(); //強迫要有查尋條件
                return blankresult;
            }



        }

        protected override void AddDBObject(IModelEntity<UserBasic> dbEntity, IEnumerable<UserBasic> objs)
        {
            // 新增到Basic的表
            var Uid = Guid.NewGuid().ToString();
            var now = DateTime.Now;

            var obj = objs.FirstOrDefault();
            if (obj == null)
            {
                //Console.WriteLine("")
                return;
            }
            string UserID = Dou.Context.CurrentUser<User>().Id;
            string PWD = Dou.Context.CurrentUser<User>().Password;
            bool IsManager = Dou.Context.CurrentUser<User>().IsManager;
            if (!IsManager)
            {
                //List<LoginInfo> Info = fun.GetInfo(UserID, PWD);
                string CityID = fun.GetInfo(UserID, PWD); // 取得登錄者的部門ID
                obj.CityID = CityID;
            }

            obj.UID = Uid;
            obj.EditTime = now;
            obj.EditName = Dou.Context.CurrentUser<User>().Id; //取得目前登入者資訊

            base.AddDBObject(dbEntity, objs);


            //新增到歷史的Basic表
            AddToHis(objs, "新增");
            //新增項目至排序表
            AddToTabulation(objs);
        }

        protected override void DeleteDBObject(IModelEntity<UserBasic> dbEntity, IEnumerable<UserBasic> objs)
        {
            base.DeleteDBObject(dbEntity, objs);

            //新增到歷史的Basic表
            AddToHis(objs, "刪除");
            // 編輯排序表
            UpdateToTabulation(objs, "刪除");

        }

        protected override void UpdateDBObject(IModelEntity<UserBasic> dbEntity, IEnumerable<UserBasic> objs)
        {
            var obj = objs.FirstOrDefault();
            if (obj == null)
            {
                //Console.WriteLine("")
                return;
            }

            obj.EditTime = DateTime.Now;
            obj.EditName = Dou.Context.CurrentUser<User>().Id;

            base.UpdateDBObject(dbEntity, objs);

            //新增到歷史的Basic表
            AddToHis(objs, "編輯");
            // 編輯排序表
            UpdateToTabulation(objs, "編輯");
        }


        

        protected void AddToHis(IEnumerable<UserBasic> objs, string Action)
        {
            var obj = objs.FirstOrDefault();
            var DataHis = new UserBasicHis
            {
                UID = obj.UID,
                Name = obj.Name,
                CityID = obj.CityID,
                PositionId = obj.PositionId,
                OfficePhone = obj.OfficePhone,
                MobilePhone = obj.MobilePhone,
                Email = obj.Email,
                Note = obj.Note,
                EditTime = obj.EditTime,
                EditName = obj.EditName,
                Action = Action
            };
            Db.UserBasicHis.Add(DataHis);
            Db.SaveChanges();
        }

        protected void AddToTabulation(IEnumerable<UserBasic> objs)
        {
            var obj = objs.FirstOrDefault();
            var CategoryList = Db.Tabulation.Where(q => q.CityID == obj.CityID).GroupBy(q => q.CategoryId)
                .Select(group => group.OrderByDescending(q => q.Sort).FirstOrDefault()).ToList();
            foreach (var item in CategoryList)
            {
                var Data = new Tabulation
                {
                    UID = obj.UID,
                    Name = obj.Name,
                    CityID = obj.CityID,
                    CategoryId = item.CategoryId,
                    Sort = item.Sort + 1,
                    Act = false
                };
                Db.Tabulation.Add(Data);
            }
            Db.SaveChanges();
        }

        protected void UpdateToTabulation(IEnumerable<UserBasic> objs, string Action)
        {
            var obj = objs.FirstOrDefault();
            var UserList = Db.Tabulation.Where(q => q.UID == obj.UID).ToList();
            var CategoryList = Db.Tabulation.Where(q => q.CityID == obj.CityID).GroupBy(q => q.CategoryId)
                .Select(group => group.OrderByDescending(q => q.Sort).FirstOrDefault()).ToList();
            // 有變更單位再更新排序表
            if (Action == "編輯")
            {
                // 單位整個變動的話 要移除舊資料再新增
                if (obj.CityID != UserList[0].CityID)
                {
                    //刪除
                    foreach (var item in UserList)
                    {
                        Db.Tabulation.Remove(item);
                    }
                    //新增
                    foreach (var item in CategoryList)
                    {
                        var Data = new Tabulation
                        {
                            UID = obj.UID,
                            Name = obj.Name,
                            CityID = obj.CityID,
                            CategoryId = item.CategoryId,
                            Sort = item.Sort + 1,
                            Act = false
                        };
                        Db.Tabulation.Add(Data);
                    }
                    Db.SaveChanges();
                }
            }
            if (Action == "刪除")
            {
                foreach (var item in UserList)
                {
                    Db.Tabulation.Remove(item);
                }
                Db.SaveChanges();
            }
        }

        
    }
}