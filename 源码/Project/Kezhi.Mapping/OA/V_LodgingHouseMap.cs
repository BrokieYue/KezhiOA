using Kezhi.Domain.Entity.OA;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Mapping.OA
{
    public class V_LodgingHouseMap : EntityTypeConfiguration<V_LodgingHouseEntity>
    {
        public V_LodgingHouseMap()
        {
            this.ToTable("V_LodgingHouse");
            this.HasKey(t => t.F_Id);
        }
    }
}
