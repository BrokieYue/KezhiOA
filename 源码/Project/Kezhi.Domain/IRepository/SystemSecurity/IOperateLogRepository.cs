/************************************************************************************
* Copyright (c) 2018 科致电气
*命名空间：Kezhi.Domain.IRepository.SystemSecurity
*文件名： IOperateLogRepository
*创建人： 劉嶺
*创建时间：2018/7/16 14:02:31
*描述
*=====================================================================
*修改标记
*修改时间：2018/7/16 14:02:31
*修改人：劉嶺
*描述：
************************************************************************************/

using Kezhi.Data;
using Kezhi.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.IRepository.SystemSecurity
{
    public interface IOperateLogRepository : IRepositoryBase<OperateLogEntity>
    {

    }
}
