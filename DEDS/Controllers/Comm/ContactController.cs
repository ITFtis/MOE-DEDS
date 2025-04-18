﻿using DEDS.Models;
using DEDS.Models.Comm;
using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEDS.Controllers.Comm
{
    [Dou.Misc.Attr.MenuDef(Id = "Contact", Name = "通聯造冊查詢", MenuPath = "緊急應變通聯手冊", Action = "Index", Index = 0, Func = Dou.Misc.Attr.FuncEnum.None, AllowAnonymous = false)]
    public class ContactController : Dou.Controllers.AGenericModelController<Tabulation>
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        protected override IModelEntity<Tabulation> GetModelEntity()
        {
            return new ModelEntity<Tabulation>(new DouModelContextExt());
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var opts = base.GetDataManagerOptions();

            opts.GetFiled("rType").filter = true;
            opts.GetFiled("Name").filter = true;
            opts.GetFiled("CityID").visible = true;
            opts.GetFiled("CategoryId").visible = true;
            opts.GetFiled("No").visible = false;
            opts.GetFiled("Act").visible = false;
            opts.GetFiled("Name").textListMatchValue = true;

            // 重新給全國縣市的列表
            opts.GetFiled("CityID").selectitems = GetALLCityIDSelectItems();
            opts.GetFiled("CategoryId").selectitems = GetALLCategoryIdSelectItems();

            return opts;
        }

        protected override IEnumerable<Tabulation> GetDataDBObject(IModelEntity<Tabulation> dbEntity, params KeyValueParams[] paras)
        {

            var rType = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "rType");
            if (rType == null)
            {
                return new List<Tabulation>();
            }

            bool IsManager = Dou.Context.CurrentUser<DEDS.Models.Manager.User>().IsManager;
            if (!IsManager)
            {
                //20240626_Brian：登入者姓名在手冊內，才能查詢(可看到全部)
                var totalTabulation = GetModelEntity().GetAll().Where(a => a.Act == true);
                string name = Dou.Context.CurrentUser<DEDS.Models.Manager.User>().Name.Trim();
                bool inBaseList = totalTabulation.Any(a => a.Name.Trim() == name);

                //20250102_Brian：有縣市編輯權限，才能查詢(可看到全部)
                var units = fun.GetUnit();
                bool isCityEdit = units.Any(a => a.CityId == Dou.Context.CurrentUser<DEDS.Models.Manager.User>().Unit);

                if (!inBaseList && !isCityEdit)
                {
                    return new List<Tabulation>();
                }
            }

            if (rType == "2")
            {
                //環保局災害應變聯繫窗口
                var iquery = GetModelEntity().GetAll();
                var datas = iquery.ToList();
                var UnitList = fun.GetUnit();

                var BaseList = Db.UserBasic.Where(a => (a.IsResContact ?? false) == true).ToList(); // 基本資料表
                var result = new List<Tabulation>();
                foreach (var UserBase in BaseList)
                {
                    var item = datas.Where(a => a.UID == UserBase.UID).FirstOrDefault();
                    if (item != null)
                    {
                        var unit = UnitList.Where(a => a.CityId == UserBase.CityID).FirstOrDefault();
                        result.Add(new Tabulation
                        {
                            No = item.No,
                            UID = item.UID,
                            Name = UserBase.Name,
                            CityID = UserBase.CityID,
                            CategoryId = item.CategoryId,
                            Sort = unit!=null ? int.Parse(unit.Id): item.Sort,
                            Act = item.Act,
                            PositionId = UserBase.PositionId,//fun.GetPositionName(PositionList, UserBase.PositionId),
                            OfficePhone = UserBase.OfficePhone,
                            MobilePhone = UserBase.MobilePhone,
                            Email = UserBase.Email,
                            Note = UserBase.Note,
                        });
                    }
                }

                result = result.OrderBy(a => a.Sort).ToList();

                return result;
            }
            else
            {
                var filterCategory = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "CategoryId");
                var filterName = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "Name");

                //強迫要有查尋條件
                if (filterCategory == null && filterName == null)
                {
                    return new List<Tabulation>();
                }
                                     
                var BaseList = Db.UserBasic.ToList(); // 基本資料表
                                                      ////var PositionList = fun.GetPosition();  

                var iquery = base.GetDataDBObject(dbEntity, paras);

                var result = new List<Tabulation>();
                iquery = iquery.Where(x => x.Act == true).OrderBy(z => z.CategoryId).ThenBy(c => c.Sort).ToList();
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
                    });
                }
                return result;
            }
            
            //return base.GetDataDBObject(dbEntity, paras).Where(x => x.Act == true).OrderBy(z => z.CategoryId).ThenBy(c => c.Sort);

        }

        public List<UserBasic> GetBase(List<UserBasic> BaseList, string UID)
        {
            List<UserBasic> result = new List<UserBasic> { BaseList.Where(q => q.UID == UID).FirstOrDefault() };
            return result;
        }

        public string GetALLCityIDSelectItems()
        {
            var JSONList = fun.GetUnit();
            string result = "{";
            foreach (var item in JSONList)
            {
                if (item.Equals(JSONList.LastOrDefault()))
                {
                    result += "\"" + item.CityId + "\":{\"v\":\"" + item.Sector + "\",\"s\":\"" + item.Id + "\"}";
                }
                else
                {
                    result += "\"" + item.CityId + "\":{\"v\":\"" + item.Sector + "\",\"s\":\"" + item.Id + "\"},";
                }
                
            }
            //string result = "{\"23\":{\"v\":\"環境部\",\"s\":\"1\"},\"2\":{\"v\":\"臺北市\",\"s\":\"2\"}}";
            result += "}";
            return result;
        }

        public string GetALLCategoryIdSelectItems()
        {
            var JSONList = fun.GetUnit();
            int index = 1;
            string result = "{";
            foreach (var item in JSONList)
            {
                foreach (var eachitem in item.Category)
                {
                    if (item.Equals(JSONList.LastOrDefault()) && eachitem.Equals(item.Category.LastOrDefault()))
                    {
                        result += "\"" + eachitem.CategoryId + "\":{\"v\":\"" + eachitem.Name + "\",\"s\":\"" + index + "\",\"CityId\":\"" + item.CityId + "\",\"Max\":\"" + eachitem.Max + "\"}";
                    }
                    else
                    {
                        result += "\"" + eachitem.CategoryId + "\":{\"v\":\"" + eachitem.Name + "\",\"s\":\"" + index + "\",\"CityId\":\"" + item.CityId + "\",\"Max\":\"" + eachitem.Max + "\"},";
                    }
                    index += 1;
                }
            }
            //opts.GetFiled("CategoryId").selectitems = "{\"CG1\":{\"v\":\"一、本部綜合規劃司環境污染事故聯繫窗口名單\",\"s\":\"0\",\"CityId\":\"23\",\"Max\":\"-\"},\"CG10\":{\"v\":\"十、臺北市環保局環境污染事故聯繫窗口名單\",\"s\":\"0\",\"CityId\":\"2\",\"Max\":\"-\"}}";

            result += "}";
            return result;


        }
    }
}