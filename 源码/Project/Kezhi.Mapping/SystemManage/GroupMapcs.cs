using Kezhi.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Mapping.SystemManage
{
    public class GroupMap : EntityTypeConfiguration<GroupEntity>
    {
        public GroupMap()
        {
            this.ToTable("Groups");
            this.HasKey(t => t.F_Id);
        }
    }
}
