/************************************************************************************
* Copyright (c) 2018 科致电气
*命名空间：Kezhi.Application.SystemSecurity
*文件名： OperateLog
*创建人： 劉嶺
*创建时间：2018/7/16 11:30:22
*描述
*=====================================================================
*修改标记
*修改时间：2018/7/16 11:30:22
*修改人：劉嶺
*描述：
************************************************************************************/

using Kezhi.Code;
using Kezhi.Domain.Entity.SystemSecurity;
using Kezhi.Domain.IRepository.SystemSecurity;
using Kezhi.Repository.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Application.SystemSecurity
{
    public class OperateLogApp
    {
        private IOperateLogRepository service = new OperateLogRepository();

        public void WriteOperateLog(OperateLogEntity operatelogEntity)
        {
            operatelogEntity.F_Id = Common.GuId();
            operatelogEntity.F_CreatorTime = DateTime.Now;
            operatelogEntity.F_CreatorUserId = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserId;
            operatelogEntity.Create();
            service.Insert(operatelogEntity);
        }
    }
}

