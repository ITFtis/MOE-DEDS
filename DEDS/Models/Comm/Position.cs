using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Comm
{
    /// <summary>
    /// 職稱代碼
    /// </summary>
    [Table("Position")]
    public class Position
    {
        ////[Key]
        ////[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        ////[Display(Name = "Id")]
        ////public int Id { get; set; }

        ////[Required]
        ////[Display(Name = "名稱")]
        ////[StringLength(50)]
        ////public string Name { get; set; }
    }
}