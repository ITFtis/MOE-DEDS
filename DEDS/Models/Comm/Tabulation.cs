using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Comm
{
    [Table("Tabulation")]
    public class Tabulation
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }


        [ColumnDef(Display = "UID",Visible = false, VisibleEdit =false)]
        public string UID { get; set; }

        [ColumnDef(Display = "姓名", Visible = true, VisibleEdit = false, EditType = EditType.TextList, SelectItemsClassNamespace = "DEDS.Models.Comm.NameSelectItems, DEDS")]
        public string Name { get; set; }

        [ColumnDef(Index = 0,Display = "單位", Visible = false, VisibleEdit = false, Filter = true, SelectGearingWith = "CategoryId,CityID,true", EditType = EditType.Select, SelectItemsClassNamespace = "DEDS.Models.Comm.CityIDSelectItems, DEDS")]
        public string CityID { get; set; }

        [ColumnDef(Index = 1, Display = "業務類別", Visible = false, VisibleEdit = false, Filter = true, EditType = EditType.Select, SelectItemsClassNamespace = "DEDS.Models.Comm.CategoryIdSelectItems, DEDS")]
        public string CategoryId { get; set; }

        [ColumnDef(Display = "排序", Visible = false, VisibleEdit = false)]
        public int Sort { get; set; }

        [ColumnDef(Display = "是否為通聯手冊造冊名單", Visible = true, VisibleEdit = false,Index = 0)]
        public bool Act { get; set; }

        // 從UserBasic表格拉出
        [ColumnDef(Display = "職稱")]
        [NotMapped]
        public string PositionId
        {
            get;
            set;
        }

        [ColumnDef(Display = "辦公室電話")]
        [NotMapped]
        public string OfficePhone
        {
            get;
            set;
        }

        [ColumnDef(Display = "行動電話")]
        [NotMapped]
        public string MobilePhone
        {
            get;
            set;
        }

        [ColumnDef(Display = "電子郵件")]
        [NotMapped]
        public string Email
        {
            get;
            set;
        }

        [ColumnDef(Display = "備註")]
        [NotMapped]
        public string Note
        {
            get;
            set;
        }

        [Display(Name = "確認日期")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public DateTime? ConfirmDate { get; set; }

        [ColumnDef(Display = "排序", Visible = false)]
        public int Order 
        { 
            get
            {
                DEDS.Controllers.Comm.ContactExportController c = new Controllers.Comm.ContactExportController();
                var vs = c.GetCategoryIdList();
                var v = vs.Where(a => a.CategoryId == this.CategoryId).FirstOrDefault();                

                return v == null ? 0 : v.Order;
            }
        }

        [Display(Name = "確認日期")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string StrConfirmDate
        {
            get
            {
                string result = "";

                if (this.ConfirmDate == null)
                {
                    return "";
                }
                else
                {
                    DateTime date = (DateTime)this.ConfirmDate;
                    result = DEDS.DateFormat.ToDate4(date) + "<br/>" + DEDS.DateFormat.ToDate12(date);
                }

                return result;
            }
        }

        [ColumnDef(Display = "聯繫類型", Visible = false, VisibleEdit = false,
            VisibleView = true,
            EditType = EditType.Select, 
            SelectItems = "{\"1\":\"各業務單位緊急應變通聯表\",\"2\":\"環保局災害應變聯繫窗口\"}"
            , DefaultValue = "1")]
        public int? rType { get; }
    }
}