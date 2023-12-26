using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DEDS.Models.Manager
{
    [Table("Role")]
    public class Role: Dou.Models.RoleBase
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "機關類別")]
        [ColumnDef(Index = 3)]
        public override string Name { get; set; }
    }
}