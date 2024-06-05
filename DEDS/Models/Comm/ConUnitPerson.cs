using Dou.Misc.Attr;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Comm
{
    /// <summary>
    /// 緊急應變單位人員
    /// </summary>
    [Table("ConUnitPerson")]
    public class ConUnitPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "應變單位")]
        [ColumnDef(EditType = EditType.Select, SelectItemsClassNamespace = DEDS.Models.Comm.ConUnitCodeItems.AssemblyQualifiedName,
                    Filter = true, FilterAssign = FilterAssignType.Equal)]
        public string ConUnit { get; set; }

        [Required]
        [Display(Name = "身分")]
        [ColumnDef(EditType = EditType.Select, SelectItemsClassNamespace = DEDS.CodeByGetConTypeItems.AssemblyQualifiedName,
                    Filter = true, FilterAssign = FilterAssignType.Equal)]
        public int ConType { get; set; }

        [Required]
        [Display(Name = "姓名")]
        [ColumnDef(Filter = true, FilterAssign = FilterAssignType.Contains)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "職稱")]
        [StringLength(50)]
        public string Position { get; set; }

        [Required]
        [Display(Name = "總機分機")]
        [StringLength(50)]
        public string Tel { get; set; }

        [Required]
        [Display(Name = "行動電話")]
        [StringLength(10)]
        public string Mobile { get; set; }

        [Display(Name = "住家電話")]
        [StringLength(50)]
        public string HTel { get; set; }

        [Required]
        [ColumnDef(Display = "EMail")]
        [StringLength(100)]
        public string EMail { set; get; }

        [Display(Name = "備註")]
        [StringLength(200)]
        [ColumnDef(EditType = EditType.TextArea)]
        public string Remark { get; set; }

        [Display(Name = "建檔日期")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public DateTime? BDate { get; set; }

        [Display(Name = "建檔人員Id")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(24)]
        public string BId { get; set; }

        [Display(Name = "建檔人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(50)]
        public string BName { get; set; }

        [Display(Name = "修改日期")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public DateTime? UDate { get; set; }

        [Display(Name = "修改人員Id")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(24)]
        public string UId { get; set; }

        [Display(Name = "修改人姓名")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        [StringLength(50)]
        public string UName { get; set; }

        [Display(Name = "確認日期")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 人員排序
        /// </summary>
        [Display(Name = "人員排序")]
        public int? PSort { get; set; }

        [Display(Name = "清單(編輯按鈕)排序")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int EditSort
        {
            get
            {
                var conUnit = Dou.Context.CurrentUser<DEDS.Models.Manager.User>().ConUnit;
                return conUnit == this.ConUnit ? 0 : 1;
            }
        }

        [Display(Name = "應變單位排序")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public int ConUnitSort
        {
            get
            {
                var code = ConUnitCodeItems.ConUnitCodes.Where(a => a.Code == this.ConUnit).FirstOrDefault();
                return code == null ? 0 : code.Sort;                
            }
        }

        [Display(Name = "確認日期")]
        [ColumnDef(Visible = false, VisibleEdit = false)]
        public string StrConfirmDate
        { 
            get
            {
                string date = this.ConfirmDate == null ? "" : this.ConfirmDate.ToString();
                if (date == null)
                    return "";

                string result = DEDS.DateFormat.ToDate4(date) + "<br/>" + DEDS.DateFormat.ToDate12(date);
                return result;
            }
        }
    }
}