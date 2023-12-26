using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Comm
{
    [Table("UserBasicHis")]
    public class UserBasicHis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false, VisibleEdit = false, VisibleView = false)]
        public int No { get; set; }

        [ColumnDef(Display = "UID", Visible = false, VisibleEdit = false)]
        public string UID { get; set; }

        [ColumnDef(Display = "姓名", Required = true)]
        public string Name { get; set; }

        [ColumnDef(Display = "單位", Visible = false, VisibleEdit = false)]
        public string CityID { get; set; }

        [ColumnDef(Display = "職稱", Required = true)]
        public string PositionId { get; set; }

        [ColumnDef(Display = "辦公室電話", Required = true)]
        public string OfficePhone { get; set; }

        [ColumnDef(Display = "行動電話", Required = true)]
        public string MobilePhone { get; set; }

        [ColumnDef(Display = "電子郵件", Required = true)]
        public string Email { get; set; }

        [ColumnDef(Display = "備註")]
        public string Note { get; set; }

        [ColumnDef(Display = "編輯時間", VisibleEdit = false)]
        public DateTime? EditTime { get; set; }

        [ColumnDef(Display = "編輯人員", VisibleEdit = false)]
        public string EditName { get; set; }

        [ColumnDef(Display = "動作", VisibleEdit = false)]
        public string Action { get; set; }

    }

    

}