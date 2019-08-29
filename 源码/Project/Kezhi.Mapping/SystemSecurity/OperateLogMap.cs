/************************************************************************************
* Copyright (c) 2018 科致电气
*命名空间：Kezhi.Mapping.SystemSecurity
*文件名： OperateLogMap
*创建人： 劉嶺
*创建时间：2018/7/16 14:04:01
*描述
*=====================================================================
*修改标记
*修改时间：2018/7/16 14:04:01
*修改人：劉嶺
*描述：
************************************************************************************/

using Kezhi.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Mapping.SystemSecurity
{
    public class OperateLogMap : EntityTypeConfiguration<OperateLogEntity>
    {
        public OperateLogMap()
        {
            this.ToTable("T_StateOperateLog");
            this.HasKey(t => t.F_Id);
        }
    }
}


