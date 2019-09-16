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
    public interface IV_ProjectRepository : IRepositoryBase<V_ProjectEntity>
    {
        List<V_ProjectEntity> GetListsNoPage(string keyword, string projectStatus);
    }
   
}
