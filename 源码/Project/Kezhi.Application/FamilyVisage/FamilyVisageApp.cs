using Kezhi.Code;
using Kezhi.Domain.Entity.FamilyVisage;
using Kezhi.Domain.IRepository.FamilyVisage;
using Kezhi.Repository.FamilyVisage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Application.FamilyVisage
{
    public class FamilyVisageApp
    {
        private IFamilyVisageRepository service = new FamilyRecordRepository();
        private IV_FamilyVisageRepository service1 = new V_FamilyRecordRepository();
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "Files\\Photo\\";

        /// <summary>
        /// 获取所有员工风采
        /// </summary>
        /// <returns></returns>
        public List<V_OA_FamilyVisageEntity> GetList()
        {
            return service1.IQueryable().ToList();
        }
        /// <summary>
        /// 根据部门和角色查询员工风采
        /// </summary>
        /// <param name="organize"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<V_OA_FamilyVisageEntity> GetListByOrganize(string organize,string role)
        {
            List<V_OA_FamilyVisageEntity> List = service1.GetListByOrganize(organize, role);
            return List;
        }
        /// <summary>
        /// 根据id获取员工风采
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<V_OA_FamilyVisageEntity> GetList(string keyword)
        {
            var expression = ExtLinq.True<V_OA_FamilyVisageEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Id.Contains(keyword));
            }
            return service1.FindList(expression, null);
        }
        public T_OA_FamilyVisageEntity GetEntity(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        /// <summary>
        /// 根据主键删除对象
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteForm(string keyValue)
        {
           
            T_OA_FamilyVisageEntity entity = service.FindEntity(keyValue);
            service.DeleteForm(keyValue);
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.F_PhotoUrl) && File.Exists(path + entity.F_PhotoUrl))
                {
                    File.Delete(path + entity.F_PhotoUrl);
                }
                if (!string.IsNullOrEmpty(entity.F_PictureUrl) && File.Exists(path + entity.F_PictureUrl))
                {
                    File.Delete(path + entity.F_PictureUrl);
                }
            }
        }
        /// <summary>
        /// 提交新增/修改数据到数据库
        /// </summary>
        /// <param name="projectEntity"></param>
        /// <param name="keyValue"></param>
        public void SubmitForm(T_OA_FamilyVisageEntity familyVisageEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                familyVisageEntity.Modify(keyValue);
            }
            else
            {
                familyVisageEntity.Create();
            }
            service.SubmitForm(familyVisageEntity, keyValue);
        }
        /// <summary>
        /// 获取员工风采序号
        /// </summary>
        /// <param name="department"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetSort(string department, string releName)
        {
            return service1.GetSort(department, releName);
        }
        /// <summary>
        /// 获取同一部门员工风采
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public List<V_OA_FamilyVisageEntity> GetFamilyByDepartment(string department)
        {
            return service1.GetFamilyByDepartment(department);
        }
        /// <summary>
        /// 根据员工id和部门获取员工风采
        /// </summary>
        /// <param name="department"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public V_OA_FamilyVisageEntity GetFamilyByUserAndDepartment(string department, string userId)
        {
            var data = service1.GetFamilyByUserAndDepartment(department, userId);
            if (data.Count > 0)
            {
                return data[0];
            }
            return null;
        }
    }
}
