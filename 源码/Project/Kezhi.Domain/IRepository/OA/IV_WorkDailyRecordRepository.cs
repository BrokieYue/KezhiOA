/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: 
 * Description: 工作日志接口
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Data;
using Kezhi.Domain.Entity.OA;
using System;
using System.Collections.Generic;

namespace Kezhi.Domain.IRepository.OA
{
    public interface IV_WorkDailyRecordRepository : IRepositoryBase<V_WorkDailyRecordEntity>
    {
        List<V_WorkDailyRecordEntity> GetLists(string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale);
        List<V_WorkDailyRecordEntity> GetWeekLists(string keyword,string projectCode, DateTime? startTime, DateTime? endTime, string organize, string filiale);
        List<V_WorkDailyRecordEntity> GetListByUserAndProject(string user, string project, DateTime? startTime, DateTime? endTime);
    }
}
