/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: 
 * Description: 工作日志接口
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Data;
using Kezhi.Domain.Entity.OA;
using System.Collections.Generic;

namespace Kezhi.Domain.IRepository.OA
{
    public interface IWorkDailyRecordRepository : IRepositoryBase<WorkDailyRecordEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(WorkDailyRecordEntity workDailyRecordEntity,string keyValue);
       List<WorkDailyRecordEntity> FindListByUserAndProject(string userId);
        List<WorkDailyRecordEntity> FindListByAll();

        List<WorkDailyRecordEntity> GetWorkDailyByWorkDate(string userId,string workDate);
    }
}
