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
    }
}
