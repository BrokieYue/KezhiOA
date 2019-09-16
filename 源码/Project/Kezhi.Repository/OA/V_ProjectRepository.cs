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
    public class V_ProjectRepository : RepositoryBase<V_ProjectEntity>, IV_ProjectRepository
    {
        public List<V_ProjectEntity> GetListsNoPage(string keyword, string projectStatus)
        {
            List<V_ProjectEntity> list = new List<V_ProjectEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"  select * FROM [KezhiOADB].[dbo].[V_OA_Project] where 1 = 1 ");
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@"and (F_ProjectCode = '"+ keyword +"' Or F_ProjectName like '%"+ keyword +"%')");
            }
            if (!string.IsNullOrEmpty(projectStatus))
            {
                if (projectStatus.Equals("暂无状态"))
                {
                    strSql.Append(@"and F_ProjectStatus = '" + projectStatus + "' or F_ProjectStatus is null");
                }
                else
                {
                    strSql.Append(@"and F_ProjectStatus = '" + projectStatus + "'");
                }
                
            }

            strSql.Append(@" order by F_CreatorTime desc");

            return this.FindList(strSql.ToString()); ;
        }
    }
}
