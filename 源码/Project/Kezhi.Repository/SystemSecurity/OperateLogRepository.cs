/************************************************************************************
* Copyright (c) 2018 科致电气
*命名空间：Kezhi.Repository.SystemSecurity
*文件名： OperateLogRepository
*创建人： 劉嶺
*创建时间：2018/7/16 14:09:59
*描述
*=====================================================================
*修改标记
*修改时间：2018/7/16 14:09:59
*修改人：劉嶺
*描述：
************************************************************************************/

using Kezhi.Data;
using Kezhi.Domain.Entity.SystemSecurity;
using Kezhi.Domain.IRepository.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Repository.SystemSecurity
{
    public class OperateLogRepository : RepositoryBase<OperateLogEntity>, IOperateLogRepository
    {

    }
}

