using Kezhi.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Kezhi.Mapping.SystemManage
{
    public class StationInfoMap : EntityTypeConfiguration<StationInfoEntity>
    {
        public StationInfoMap()
        {
            this.ToTable("UDT_PM_StationInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
