/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Code;
using Kezhi.Data;
using Kezhi.Domain.Entity.OA;
using Kezhi.Domain.IRepository.OA;
using System.Collections.Generic;
using System.Text;

namespace Kezhi.Repository.OA
{
    public class WorkDailyRecordRepository : RepositoryBase<WorkDailyRecordEntity>, IWorkDailyRecordRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<WorkDailyRecordEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(WorkDailyRecordEntity workDailyRecordEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(workDailyRecordEntity);
                }
                else
                {
                    db.Insert(workDailyRecordEntity);
                }
                db.Commit();
            }
        }
        public List<WorkDailyRecordEntity> FindListByUserAndProject(string userId)
        {
            List<WorkDailyRecordEntity> list = new List<WorkDailyRecordEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from T_OA_WorkDailyRecord where 1 = 1 ");
            if (!string.IsNullOrEmpty(userId) && !userId.Equals(" "))
            {
                strSql.Append(@"and [F_WorkUserId] = '" + userId + "'");
            }
            strSql.Append(@" order by F_WorkDate desc,F_CreatorTime desc");

            return this.FindList(strSql.ToString()); ;
        }
        public List<WorkDailyRecordEntity> FindListByAll()
        {
            List<WorkDailyRecordEntity> list = new List<WorkDailyRecordEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from T_OA_WorkDailyRecord order by F_WorkDate desc,F_WorkUserId desc");
        
            return this.FindList(strSql.ToString()); 
        }
        public List<WorkDailyRecordEntity> GetWorkDailyByWorkDate(string userId,string workDate)
        {
            List<WorkDailyRecordEntity> list = new List<WorkDailyRecordEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from T_OA_WorkDailyRecord where F_WorkDate = '" + workDate + "' and F_WorkUserId = '" + userId + "'");

            return this.FindList(strSql.ToString()); 
        }

    }
}
