using Kezhi.Code;
/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kezhi.Application.SystemManage
{
    public class AreaApp
    {
        private IAreaRepository service = new AreaRepository();

        public List<AreaEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public AreaEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(AreaEntity areaEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                areaEntity.Modify(keyValue);
                service.Update(areaEntity);
            }
            else
            {
                areaEntity.Create();
                service.Insert(areaEntity);
            }
        }

        public List<AreaEntity> GetListGrading(string layers, string parentId)
        {
            var expression = ExtLinq.True<AreaEntity>();
            if (!string.IsNullOrEmpty(layers))
            {
                int layer = int.Parse(layers);
                expression = expression.And(t => t.F_Layers == layer);
            }
            if (!string.IsNullOrEmpty(parentId))
            {
                expression = expression.And(t => t.F_ParentId.Contains(parentId));
            }
            return service.IQueryable(expression).ToList();
        }

    }
}
