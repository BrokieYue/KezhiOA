using Kezhi.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Kezhi.Mapping.SystemManage
{
    public class LineAreaMap : EntityTypeConfiguration<LineAreaEntity>
    {
        public LineAreaMap()
        {
            this.ToTable("UDT_PM_LineArea");
            this.HasKey(t => t.F_Id);
        }
    }
}
