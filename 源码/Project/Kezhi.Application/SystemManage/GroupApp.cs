using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kezhi.Code;

namespace Kezhi.Application.SystemManage
{
    public class GroupApp
    {
        private IGroupRepository service = new GroupRepository();

        public List<GroupEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<GroupEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.GroupName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }
        /// <summary>
        /// 获取所有班组
        /// </summary>
        /// <returns></returns>
        public List<GroupEntity> GetList() 
        {
            return service.FindList("select * from Groups");
        }

        public GroupEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(GroupEntity GroupEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                GroupEntity.Modify(keyValue);
                service.Update(GroupEntity);
            }
            else
            {
                GroupEntity.Create();
                service.Insert(GroupEntity);
            }
        }
    }
}
