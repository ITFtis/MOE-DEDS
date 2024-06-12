using DEDS.Models.Manager;
using Dou.Misc.Attr;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Comm
{
    [Table("UserBasic")]
    public class UserBasic
    {
        [Key]
        [ColumnDef(Display = "UID", Visible = false, VisibleEdit = false)]
        public virtual string UID { get; set; }

        [ColumnDef(Display = "姓名", Index = 1, Required = true, Filter = true, EditType = EditType.TextList, SelectItemsClassNamespace = "DEDS.Models.Comm.NameSelectItems, DEDS")]
        [StringLength(200)]
        public string Name { get; set; }

        [ColumnDef(Display = "單位", Index = 0, Visible = false, Required = true, VisibleEdit = true, Filter = true, SelectGearingWith = "Name,CityID,false", EditType = EditType.Select, SelectItemsClassNamespace = "DEDS.Models.Comm.CityIDSelectItems, DEDS")]
        public string CityID { get; set; }

        [ColumnDef(Display = "職稱", Required = true, EditType = EditType.TextList, TextListMatchValue = true, SelectItemsClassNamespace = "DEDS.Models.Comm.PositionSelectItems, DEDS")]
        public string PositionId { get; set; }

        [ColumnDef(Display = "辦公室電話", VisibleEdit = false)]
        public string OfficePhone { get; set; }


        [ColumnDef(Display = "辦公室電話", Required = true, Visible = false)] // 為了把辦公室電話 分成三份
        [NotMapped]
        public string OfficePhone1
        {
            get
            {
                if (OfficePhone != null)
                {
                    return OfficePhone.Split('-')[0];
                }
                else { return OfficePhone; }

            }
            set
            {
                OfficePhone_ = value;
            }

        }

        [ColumnDef(Display = "-", Required = true, Visible = false)] // 區碼以外的部分
        [NotMapped]
        public string OfficePhone2
        {
            get
            {
                if (OfficePhone != null)
                {
                    return OfficePhone.Split('-')[1].Split('#')[0];
                }
                else { return OfficePhone; }
            }
            set
            {
                OfficePhone_ += "-" + value;
            }

        }

        [ColumnDef(Display = "#", Visible = false)] //分機號碼
        [NotMapped]
        public string OfficePhone3
        {
            get
            {
                if (OfficePhone != null)
                {
                    if (OfficePhone.Contains("#") == true)
                    {
                        return OfficePhone.Split('-')[1].Split('#')[1];
                    }
                    else { return ""; }
                }
                else { return ""; }
            }
            set
            {

                OfficePhone_ += "#" + value;
                OfficePhone = OfficePhone_;
            }
        }

        [ColumnDef(Visible = false, VisibleEdit = false)] //暫存電話號碼欄位
        [NotMapped]
        public string OfficePhone_ { get; set; }

        [ColumnDef(Display = "行動電話", Required = true)]
        public string MobilePhone { get; set; }

        [ColumnDef(Display = "電子郵件", Required = true, EditType = EditType.Email)]
        public string Email { get; set; }

        [ColumnDef(Display = "備註")]
        public string Note { get; set; }

        [ColumnDef(Display = "編輯時間", VisibleEdit = false)]
        public DateTime? EditTime { get; set; }

        [ColumnDef(Display = "編輯人員", VisibleEdit = false)]
        public string EditName { get; set; }
    }

    public class PositionSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public Function fun = new Function();
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            var JSONList = fun.GetPosition();
            return JSONList.Select(
                            s => new KeyValuePair<string, object>(s.PositionId, s.Name)
                            );
        }


    }

    public class CityIDSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            var JSONList = fun.GetUnit();
            string UserID = Dou.Context.CurrentUser<User>().Id;
            string PWD = Dou.Context.CurrentUser<User>().Password;
            bool IsManager = Dou.Context.CurrentUser<User>().IsManager;
            List<KeyValuePair<string, object>> keyValuePair = new List<KeyValuePair<string, object>>();
            if (!IsManager)
            {

                //List<LoginInfo> Info = fun.GetInfo(UserID, PWD);
                string CityID = fun.GetInfo(UserID, PWD); // 取得登錄者的部門ID
                var targetCityUnits = JSONList.Where(unit => unit.CityId == CityID).ToList();

                keyValuePair = targetCityUnits.Select(s => new KeyValuePair<string, object>(s.CityId ,
                JsonConvert.SerializeObject(new { v = s.Sector, s = s.Id }))).ToList();

                return keyValuePair;
            }
            else
            {
                keyValuePair = JSONList.Select(s => new KeyValuePair<string, object>(s.CityId ,
                JsonConvert.SerializeObject(new { v = s.Sector, s = s.Id }))).ToList();

                return keyValuePair;
            }
        }


    }

    public class CategoryIdSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            var JSONList = fun.GetUnit();
            string UserID = Dou.Context.CurrentUser<User>().Id;
            string PWD = Dou.Context.CurrentUser<User>().Password;
            bool IsManager = Dou.Context.CurrentUser<User>().IsManager;
            List<KeyValuePair<string, object>> keyValuePair = new List<KeyValuePair<string, object>>();
            int index = 1;
            // 找到 CityID 為 "1" 的 Unit
            if (!IsManager)
            {
                //List<LoginInfo> Info = fun.GetInfo(UserID, PWD);
                string CityID = fun.GetInfo(UserID, PWD); // 取得登錄者的部門ID
                var targetCityUnits = JSONList.Where(unit => unit.CityId == CityID).ToList();
                var targetCityCategories = targetCityUnits.FirstOrDefault().Category;

                foreach (var item in targetCityUnits)
                {
                    foreach (var eachitem in item.Category)
                    {
                        keyValuePair.Add(new KeyValuePair<string, object>(eachitem.CategoryId 
                            , JsonConvert.SerializeObject(new { v = eachitem.Name, CityId = item.CityId, s = index, Max = eachitem.Max })));
                        index += 1;
                    }
                }

                return keyValuePair;
            }
            else
            {
                foreach (var item in JSONList)
                {
                    foreach (var eachitem in item.Category)
                    {
                        keyValuePair.Add(new KeyValuePair<string, object>(eachitem.CategoryId 
                            , JsonConvert.SerializeObject(new { v = eachitem.Name, CityId = item.CityId, s = index, Max = eachitem.Max })));
                        index += 1;
                    }
                }
                return keyValuePair;
            }

        }
    }

    public class NameSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            var UserBasic = Db.UserBasic.OrderByDescending(w => w.CityID).ThenBy(w => w.PositionId).ToList();
            string UserID = Dou.Context.CurrentUser<User>().Id;
            string PWD = Dou.Context.CurrentUser<User>().Password;
            bool IsManager = Dou.Context.CurrentUser<User>().IsManager;
            List<KeyValuePair<string, object>> keyValuePair = new List<KeyValuePair<string, object>>();
            int index = 1;
            var PositionList = fun.GetPosition();
            if (!IsManager) //一般使用者
            {
                //List<LoginInfo> Info = fun.GetInfo(UserID, PWD);
                string CityID = fun.GetInfo(UserID, PWD); // 取得登錄者的部門ID
                var TargetCityUsers = UserBasic.Where(r => r.CityID == CityID).ToList();

                foreach (var item in TargetCityUsers)
                {
                    keyValuePair.Add(new KeyValuePair<string, object>(item.Name
                        , JsonConvert.SerializeObject(new { v = item.Name, CityId = item.CityID, s = item.PositionId })));
                }

                return keyValuePair;
            }
            else
            {
                foreach (var item in UserBasic)
                {
                    keyValuePair.Add(new KeyValuePair<string, object>(item.Name
                        , JsonConvert.SerializeObject(new { v = item.Name, CityId = item.CityID, s = index })));
                    index += 1;
                }
                return keyValuePair;
            }

        }


    }

    public class Function
    {
        public DouModelContextExt Db = new DouModelContextExt();

        //要改CityID取得方式 用User的Unit欄位
        public string GetInfo(string UserID, string PWD)
        {
            var cityID = Db.User.Where(q => q.Id == UserID).Select(w => w.Unit).FirstOrDefault();
            bool Isint = int.TryParse(cityID, out int result);
            if (Isint == false)
            {
                var options = new RestClientOptions(Startup.AppSet.OldSysUserLoginApi)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(Startup.AppSet.OldSysUserLoginApi, Method.Post);
                request.AddHeader("token", "aS6bQK2Bj4rS[awqFY&"); //預防不是從系統Post的路徑呼叫的
                request.AddHeader("Content-Type", "application/json");
                var body = @"{""UserName"":""" + UserID + @""",""Pwd"":""" + PWD + @"""}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = client.Execute(request);
                var jsonstring = response.Content;
                var Result = JsonConvert.DeserializeObject<List<LoginInfo>>(jsonstring);
                return Result[0].CityId.ToString();
            }
            else
            {
                return cityID;
            }
            
        }

        static object LockGetAllDep = new object();

        public List<Unit> GetUnit()
        {

            lock (LockGetAllDep)
            {
                var r = DouHelper.Misc.GetCache<IEnumerable<Unit>>(60 * 1000);
                if (r == null)
                {
                    r = DouHelper.Misc.DeSerializeObjectLoadJsonFile<IEnumerable<Unit>>(System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/Data/Comm")), "Unit.json"));
                    DouHelper.Misc.AddCache(r);
                    
                }
                var Result = r.ToList();
                return Result;
            }
        }

        public static List<DocDot> GetDocDot()
        {

            lock (LockGetAllDep)
            {
                //No catch(不影響效能下,方便調整測試)
                var r = DouHelper.Misc.DeSerializeObjectLoadJsonFile<IEnumerable<DocDot>>(System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/Data/Comm")), "DocDot.json"));
                var Result = r.ToList();
                return Result;
            }
        }

        public List<Position> GetPosition()
        {
            lock (LockGetAllDep)
            {
                var r = DouHelper.Misc.GetCache<IEnumerable<Position>>(60 * 1000);
                if (r == null)
                {
                    r = DouHelper.Misc.DeSerializeObjectLoadJsonFile<IEnumerable<Position>>(System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/Data/Comm")), "Position.json"));
                    DouHelper.Misc.AddCache(r);

                }
                var Result = r.ToList();
                return Result;
            }
        }

        public string GetPositionName(List<Position> List, string PositionId)
        {
            return List.Where(w => w.PositionId == PositionId).Select(w => w.Name).FirstOrDefault();
        }
    }


    #region class
    public class Position
    {
        public string PositionId { get; set; }
        public string Name { get; set; }
    }

    public class Category
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }

        public string Max { get; set; }
    }

    public class Unit
    {
        public string Id { get; set; }
        public string CityId { get; set; }
        public string Sector { get; set; }
        public List<Category> Category { get; set; }
    }

    public class DocDot
    {
        public string Id { get; set; }
        public string Name { get; set; }        
        public string[] Dot { get; set; }
    }

    public class LoginInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public object MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public string Email { get; set; }
    }
    #endregion
}