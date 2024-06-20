using DEDS.Models.Manager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DEDS.Models
{
    public partial class DouModelContextExt : Dou.Models.ModelContextBase<User, Role>
    {
        public DouModelContextExt()
            : base("name=DouModelContextExt")
        {
            Database.SetInitializer<DouModelContextExt>(null);
        }
        
        public virtual DbSet<Comm.Tabulation> Tabulation { get; set; }
        public virtual DbSet<Comm.UserBasic> UserBasic { get; set; }
        public virtual DbSet<Comm.UserBasicHis> UserBasicHis { get; set; }
        public virtual DbSet<Comm.ConUnitPerson> ConUnitPerson { get; set; }

        public virtual DbSet<Comm.ConUnitCode> ConUnitCode { get; set; }

        //public virtual DbSet<Comm.Position> Position { get; set; }
    }
}