﻿using Antlr.Runtime.Misc;
using DEDS.Models.Comm;
using Dou.Misc.Attr;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Manager
{
    [Table("User")]
    public class User : Dou.Models.UserBaseExt
    {
        [ColumnDef(Display = "名稱", Filter = true, FilterAssign = FilterAssignType.Contains, Index = 2)]
        [StringLength(200)]
        public override string Name { set; get; }



        [ColumnDef(Display = "機關", Index = 20)]
        public string Organize { set; get; }

        [ColumnDef(Display = "單位", Index = 22, EditType = EditType.Select, SelectItemsClassNamespace = "DEDS.Models.Manager.ALLCityIDSelectItems, DEDS")]
        public string Unit { set; get; }
        [ColumnDef(Display = "部門", Index = 24)]
        public string SubUnit { set; get; }

        [Display(Name = "緊急應變單位")]
        [ColumnDef(EditType = EditType.Select, SelectItemsClassNamespace = DEDS.Models.Comm.ConUnitCodeItems.AssemblyQualifiedName,
                    Filter = false, FilterAssign = FilterAssignType.Equal,
                    Index = 25)]
        public string ConUnit { get; set; }

        [ColumnDef(Display = "手機", Index = 26)]
        public string Mobile { set; get; }
        [ColumnDef(Display = "電話", Index = 28)]
        public string Tel { set; get; }
        [ColumnDef(Display = "EMail", Index = 30)]
        public string EMail { set; get; }
        [ColumnDef(Display = "傳真", Index = 32)]
        public string Fax { set; get; }
        //[ColumnDef(Display = "是否權責人員", Index = 33)]
        //public bool IsManager { set; get; }

        [ColumnDef(Display = "權責人員", Required = true, EditType = EditType.Select, SelectItems = "{\"true\":\"是\",\"false\":\"否\"}", DefaultValue = "false")]
        public bool IsManager { get; set; }

        [ColumnDef(Display = "是否同一單位(通聯單位=緊急應變單位)", Visible = false, VisibleEdit = false)]
        public bool IsCommEditPower
        {
            get
            {
                bool result = false;

                if (Dou.Context.CurrentUser<User>() == null)
                    return false;

                //登入者單位
                string LoginUnit = Dou.Context.CurrentUser<User>().Unit;                

                //通聯手冊單位
                Function fun = new Function();
                var unit = fun.GetUnit().Where(a => a.Id == this.Unit).FirstOrDefault();
                if (unit == null)
                    return false;

                if (LoginUnit == "23")
                {
                    //登入者為環境部單位
                    return true;
                    //////通聯單位比登入者單位(Id)
                    ////if(unit.CityId == LoginUnit)
                    ////    result = true;
                }
                else
                {
                    //登入者非環保局單位
                    //通聯單位比應變單位(Name)
                    var conUnit = ConUnitCode.GetAllDatas().Where(a => a.Code == this.ConUnit).FirstOrDefault();
                    if (conUnit == null)
                        return false;

                    result = conUnit.Name == unit.Sector;
                }

                return result;
            }
        }
    }

    public class ALLCityIDSelectItems : Dou.Misc.Attr.SelectItemsClass
    {
        public DouModelContextExt Db = new DouModelContextExt();
        public Function fun = new Function();
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            var JSONList = fun.GetUnit();
            List<KeyValuePair<string, object>> keyValuePair = new List<KeyValuePair<string, object>>();

            keyValuePair = JSONList.Select(s => new KeyValuePair<string, object>(s.CityId,
            JsonConvert.SerializeObject(new { v = s.Sector, s = s.Id }))).ToList();

            return keyValuePair;

        }


    }
    
}