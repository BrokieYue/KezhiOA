/*******************************************************************************
 * Copyright © 2016 科致电气
 * Author: GeneralCheng
 * Description: 长安渝北工厂焊装车间WIMS系统
 * Website：www.kezhi-electric.com
*********************************************************************************/
using NFine.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SystemManage
{
    public class CarMaintenanceMap : EntityTypeConfiguration<CarMaintenanceEntity>
    {
        public CarMaintenanceMap()
        {
            this.ToTable("Car_Code");
            this.HasKey(t => t.id);
        }
    }
}
