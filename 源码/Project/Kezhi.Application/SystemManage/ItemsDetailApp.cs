/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Code;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace Kezhi.Application.SystemManage
{
    public class ItemsDetailApp
    {
        private IItemsDetailRepository service = new ItemsDetailRepository();

        public List<ItemsDetailEntity> GetList(string itemId = "", string keyword = "")
        {
            var expression = ExtLinq.True<ItemsDetailEntity>();
            if (!string.IsNullOrEmpty(itemId))
            {
                expression = expression.And(t => t.F_ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_ItemName.Contains(keyword));
                expression = expression.Or(t => t.F_ItemCode.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.F_SortCode).ToList();
        }
        public List<ItemsDetailEntity> GetItemList(string enCode)
        {
            return service.GetItemList(enCode);
        }
        public ItemsDetailEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        /// <summary>
        /// 主要用于项目类型插叙
        /// 根据其他任务查询项目类型
        /// </summary>
        /// <param name="simple"></param>
        /// <returns></returns>
        public List<ItemsDetailEntity> GetItemListBySimple(string simple)
        {
            var expression = ExtLinq.True<ItemsDetailEntity>();
            if (!string.IsNullOrEmpty(simple))
            {
                expression = expression.And(t => t.F_SimpleSpelling == simple);
            }
            return service.IQueryable(expression).OrderBy(t => t.F_SortCode).ToList();
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsDetailEntity.Modify(keyValue);
                service.Update(itemsDetailEntity);
            }
            else
            {
                itemsDetailEntity.Create();
                service.Insert(itemsDetailEntity);
            }
        }

        /// <summary>
        /// 根据项目类型
        /// </summary>
        /// <param name="projectType"></param>
        /// <returns></returns>
        public ItemsDetailEntity GetItemsByProjectType(string projectType)
        {
            //项目类型
           ItemsDetailEntity item = service.FindEntity(projectType);
           if (item == null)
           {
               return null;
           }
           var simple = item.F_SimpleSpelling;
            //工作类型
           if (!string.IsNullOrEmpty(simple))
           {
              item = service.FindEntity(simple);
           }

           return item;
        }
    }
}
