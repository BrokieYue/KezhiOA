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
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Kezhi.Repository.OA
{
    
    public class V_WorkDailyRecordRepository : RepositoryBase<V_WorkDailyRecordEntity>, IV_WorkDailyRecordRepository
    {
        public List<V_WorkDailyRecordEntity> GetLists(string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale)
        {
            List<V_WorkDailyRecordEntity> list = new List<V_WorkDailyRecordEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from V_OA_WorkDailyRecord where 1 = 1 ");
            if (!string.IsNullOrEmpty(keyword) && !keyword.Equals(" "))
            {
                strSql.Append(@"and [F_WorkUserName] = '" + keyword + "'");
            }
           
            if (startTime != null && endTime != null)
            {
                strSql.Append(@" and [F_WorkDate] >= '" + startTime + "' and [F_WorkDate] <= '" + endTime + "' ");
            }
            else
            {
                DateTime currentMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                strSql.Append(@" and [F_WorkDate] >= '" + currentMonth + "'");
            }
            if (!string.IsNullOrEmpty(organize) && !organize.Equals(" "))
            {
                var organizeName = organize.Trim();
                strSql.Append(@"and [F_OrganizeName] = '" + organizeName + "'");
            }
            if (!string.IsNullOrEmpty(filiale) && !filiale.Equals(" "))
            {
                strSql.Append(@"and [F_FilialeId] = '" + filiale + "'");
            }
            //strSql.Append(@" order by F_WorkUserId desc,F_WorkDate asc");
            strSql.Append(@" order by F_WorkUserId ,F_WorkDate asc");

            return this.FindList(strSql.ToString()); ;

            
        }
        public List<V_WorkDailyRecordEntity> GetWeekLists(string keyword, string projectCode, DateTime? startTime, DateTime? endTime, string organize, string filiale)
        {
            List<V_WorkDailyRecordEntity> list = new List<V_WorkDailyRecordEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from V_OA_WorkDailyRecord where 1 = 1 ");
            //strSql.Append(@"SELECT [F_Id],[F_WorkUserName],[F_WorkDate],[F_ProjectName],[F_WorkAddressCode],[F_WorkedHours],[F_PayHours],[F_RestHours],[F_DeductHours],[F_MealSubsidy],[F_WorkSubsidy]");
            //strSql.Append(@",[F_DailyRecord],[F_WorkUserId],[F_ProjectId],[F_WorkAddress],[F_WorkType],[F_WorkTimeStart],[F_WorkTimeEnd],[F_Description],[F_EnabledMark],[F_DeleteMark],[F_CreatorTime]");
            //strSql.Append(@",[F_CreatorUserId],[F_DeleteUserId],[F_ProjectCode],[F_CreateorUserName] ,[F_LastModifyUserName],[F_DeleteUserName],[F_CreatType],[F_IfLocal],[F_IfLocal],[F_LastModifyTime],[F_LastModifyUserId],[F_DeleteTime] from V_OA_WorkDailyRecord where 1 = 1 ");
            if (!string.IsNullOrEmpty(keyword) && !keyword.Equals(" "))
            {
                strSql.Append(@"and [F_WorkUserName] = '" + keyword + "'");
            }
            if (!string.IsNullOrEmpty(projectCode) && !projectCode.Equals(" "))
            {
                strSql.Append(@"and [F_ProjectCode] = '" + projectCode + "'");
            }
            if (startTime != null && endTime != null)
            {
                strSql.Append(@" and [F_WorkDate] >= '" + startTime + "' and [F_WorkDate] <= '" + endTime + "' ");
            }
            else
            {
                DateTime currentMonth = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                strSql.Append(@" and [F_WorkDate] >= '" + currentMonth + "'");
            }
            if (!string.IsNullOrEmpty(organize) && !organize.Equals(" "))
            {
                var organizeName = organize.Trim();
                strSql.Append(@"and [F_OrganizeName] = '" + organizeName + "'");
            }
            if (!string.IsNullOrEmpty(filiale) && !filiale.Equals(" "))
            {
                strSql.Append(@"and [F_FilialeId] = '" + filiale + "'");
            }
            //strSql.Append(@" order by F_WorkUserId desc,F_WorkDate asc");
            strSql.Append(@" order by F_ProjectCode,F_WorkUserId,F_WorkDate asc");

            return this.FindList(strSql.ToString()); 


        }

        public List<V_WorkDailyRecordEntity> GetListByUserAndProject(string user, string project, DateTime? startTime, DateTime? endTime)
        {
            List<V_WorkDailyRecordEntity> list = new List<V_WorkDailyRecordEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from V_OA_WorkDailyRecord where 1 = 1 ");
            if (!string.IsNullOrEmpty(user) && !user.Equals(" "))
            {
                strSql.Append(@"and [F_WorkUserId] = '" + user + "'");
            }
            if (!string.IsNullOrEmpty(project) && !project.Equals(" "))
            {
                strSql.Append(@"and [F_ProjectId] = '" + project + "'");
            }
            if (startTime != null && endTime != null)
            {
                strSql.Append(@" and [F_WorkDate] >= '" + startTime + "' and [F_WorkDate] <= '" + endTime + "' ");
            }
            //strSql.Append(@" order by F_WorkUserId desc,F_WorkDate asc");
            strSql.Append(@" order by F_ProjectId,F_WorkDate asc");

            return this.FindList(strSql.ToString()); 
        }
    }
}
