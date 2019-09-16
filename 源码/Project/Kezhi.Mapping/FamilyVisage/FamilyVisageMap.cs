using Kezhi.Domain.Entity.FamilyVisage;
using Kezhi.Domain.Entity.OA;
/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using System.Data.Entity.ModelConfiguration;

namespace Kezhi.Mapping.OA
{
    public class FamilyVisageMap : EntityTypeConfiguration<T_OA_FamilyVisageEntity>
    {
        public FamilyVisageMap()
        {
            this.ToTable("T_OA_FamilyVisage");
            this.HasKey(t => t.F_Id);
        }
    }
}
