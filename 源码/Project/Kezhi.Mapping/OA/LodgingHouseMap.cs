using Kezhi.Domain.Entity.OA;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Mapping.OA
{
    public class LodgingHouseMap : EntityTypeConfiguration<LodgingHouseEntity>
    {
        public LodgingHouseMap()
        {
            this.ToTable("T_OA_LodgingHouse");
            this.HasKey(t => t.F_Id);
        }
    }
}
