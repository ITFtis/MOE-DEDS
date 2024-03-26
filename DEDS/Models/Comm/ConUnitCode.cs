using DEDS.Models.Manager;
using DouHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Comm
{
    /// <summary>
    /// 緊急應變單位代碼
    /// </summary>
    [Table("ConUnitCode")]
    public class ConUnitCode
    {
        [Key]
        [Display(Name = "部門代碼")]
        [StringLength(10)]        
        public string Code { get; set; }

        [Required]
        [Display(Name = "部門名稱")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }       
    }

    public class ConUnitCodeItems : Dou.Misc.Attr.SelectItemsClass
    {
        public const string AssemblyQualifiedName = "DEDS.Models.Comm.ConUnitCodeItems, DEDS";

        protected static IEnumerable<ConUnitCode> _conUnitCode;
        internal static IEnumerable<ConUnitCode> ConUnitCodes
        {
            get
            {
                if (_conUnitCode == null)
                {
                    using (var db = new DouModelContextExt())
                    {
                        string ConUnit = Dou.Context.CurrentUser<User>().ConUnit;
                        bool IsManager = Dou.Context.CurrentUser<User>().IsManager;

                        var datas = db.ConUnitCode.AsEnumerable();
                        if (IsManager)
                        {
                        }
                        else if (ConUnit != null)
                        {
                            datas = datas.Where(a => a.Code == ConUnit);
                        }
                        else
                        {
                            datas = datas.Take(0);
                        }

                        _conUnitCode = datas.OrderBy(a => a.Sort).ToArray();
                    }
                }
                return _conUnitCode;
            }
        }


        public static void Reset()
        {
            _conUnitCode = null;
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {            
            return ConUnitCodes.Select(s => new KeyValuePair<string, object>(s.Code, s.Name));
        }
    }
}