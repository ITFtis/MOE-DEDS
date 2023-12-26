using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Manager
{
    [Table("User")]
    public class User: Dou.Models.UserBaseExt
    {
        [ColumnDef(Display = "名稱", Index = 2)]
        public override string Name { set; get; }

        

        [ColumnDef(Display = "機關", Index = 20)]
        public string Organize { set; get; }

        [ColumnDef(Display = "單位", Index = 22)]
        public string Unit { set; get; }
        [ColumnDef(Display = "部門", Index = 24)]
        public string SubUnit { set; get; }
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



    }
}