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
    public class V_WorkDailyRecordMap : EntityTypeConfiguration<V_WorkDailyRecordEntity>
    {
        public V_WorkDailyRecordMap()
        {
            this.ToTable("V_OA_WorkDailyRecord");
            this.HasKey(t => t.F_Id);
        }
    }
}
