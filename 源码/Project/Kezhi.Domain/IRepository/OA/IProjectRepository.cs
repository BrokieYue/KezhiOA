/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Data;
using Kezhi.Domain.Entity.OA;
using System.Collections.Generic;

namespace Kezhi.Domain.IRepository.OA
{
    public interface IProjectRepository : IRepositoryBase<ProjectEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(ProjectEntity projectEntity,string keyValue);

        List<ProjectEntity> GetListOrderByDate();

        List<ProjectEntity> GetListByStatus(string[] status);
    }
}
