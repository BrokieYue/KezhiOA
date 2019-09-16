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
    public class ProjectRepository : RepositoryBase<ProjectEntity>, IProjectRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ProjectEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(ProjectEntity projectEntity,  string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(projectEntity);
                }
                else
                {
                    db.Insert(projectEntity);
                }
                db.Commit();
            }
        }
        public List<ProjectEntity> GetListOrderByDate()
        {
            List<ProjectEntity> list = new List<ProjectEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM [KezhiOADB].[dbo].[T_OA_Project] order by F_ProjectCode, F_CreatorTime desc ");
           
            return this.FindList(strSql.ToString()); 
        }

        public List<ProjectEntity> GetListByStatus(string[] status)
        {
            List<ProjectEntity> list = new List<ProjectEntity>();
            StringBuilder strSql = new StringBuilder();

            strSql.Append(@"SELECT * FROM [KezhiOADB].[dbo].[T_OA_Project] where 1 =1  ");
            if (status.Length > 0)
            {
                for (var i = 0; i < status.Length; i++)
                {
                    if (i == 0)
                    {
                        strSql.Append(@" and F_ProjectStatus != '"+ status[0] +"'");
                    }
                    else
                    {
                        strSql.Append(@"Or F_ProjectStatus != '"+ status[i] +"'");
                    }
                    
                }
                strSql.Append(@"OR F_ProjectStatus is null");
            }
            return this.FindList(strSql.ToString());
        }
    }
}
