using DEDS.Models.Manager;
using Dou.Misc.Attr;
using DouHelper;
using Newtonsoft.Json;
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

        //組織查詢(大單位組織1)
        [Display(Name = "組織查詢")]
        [ColumnDef(EditType = EditType.Select,
            Filter = true, SelectItemsClassNamespace = DEDS.CusOrg1SelectItems.AssemblyQualifiedName)]
        public int CusOrg1 { get; set; }

        static object lockGetAllDatas = new object();
        public static IEnumerable<ConUnitCode> GetAllDatas(int cachetimer = 0)
        {
            if (cachetimer == 0) cachetimer = Constant.cacheTime;

            string key = "DEDS.Models.Comm.ConUnitCode";
            var allData = DouHelper.Misc.GetCache<IEnumerable<ConUnitCode>>(cachetimer, key);
            lock (lockGetAllDatas)
            {
                if (allData == null)
                {
                    Dou.Models.DB.IModelEntity<ConUnitCode> modle = new Dou.Models.DB.ModelEntity<ConUnitCode>(new DouModelContextExt());
                    allData = modle.GetAll().OrderBy(a => a.Sort).ToArray();

                    DouHelper.Misc.AddCache(allData, key);
                }
            }

            return allData;
        }

        public static void ResetGetAllDatas()
        {
            string key = "DEDS.Models.Comm.ConUnitCode";
            Misc.ClearCache(key);
        }
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
                    var datas = ConUnitCode.GetAllDatas();
                    _conUnitCode = datas.OrderBy(a => a.Sort).ToArray();
                }
                return _conUnitCode;
            }
        }


        public static void Reset()
        {
            _conUnitCode = null;
            ConUnitCode.ResetGetAllDatas();
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            //return ConUnitCodes.Select(s => new KeyValuePair<string, object>(s.Code, s.Name));            
            return ConUnitCodes.Select(s => new KeyValuePair<string, object>(s.Code, JsonConvert.SerializeObject(new { v = s.Name, CusOrg1 = s.CusOrg1 })));
        }
    }
}