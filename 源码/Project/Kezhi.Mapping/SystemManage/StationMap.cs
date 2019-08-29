using Kezhi.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Kezhi.Mapping.SystemManage
{
    public class StationMap : EntityTypeConfiguration<StationEntity>
    {
        public StationMap()
        {
            this.ToTable("UDT_PM_Station");
            this.HasKey(t => t.F_Id);
        }
    }
}
