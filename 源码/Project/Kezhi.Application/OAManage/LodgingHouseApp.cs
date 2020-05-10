using Kezhi.Code;
using Kezhi.Domain.Entity.OA;
using Kezhi.Domain.IRepository.OA;
using Kezhi.Repository.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Application.OAManage
{
    public class LodgingHouseApp
    {
        private ILodgingHouseRepository service = new LodgingHouseRepository();
        private IV_LodgingHouseRepository v_service = new V_LodgingHouseRepository();


        /// <summary>
        /// 获取所有宿舍信息
        /// </summary>
        /// <returns></returns>
        public List<LodgingHouseEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        /// <summary>
        /// 分页查询（按条件）
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<V_LodgingHouseEntity> GetList(Pagination pagination, string keyword, string projectCode)
        {
            var expression = ExtLinq.True<V_LodgingHouseEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_HouseCode.Contains(keyword));
                expression = expression.Or(t => t.F_HouseName.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(projectCode))
            {
                expression = expression.And(t => t.F_ProjectId.Contains(projectCode));
                expression = expression.Or(t => t.F_ProjectName.Contains(projectCode));
            }
            expression = expression.And(t => t.F_EnabledMark == true);
            return v_service.FindList(expression, pagination);
        }
        /// <summary>
        /// 不分页查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public List<V_LodgingHouseEntity> GetLists(string keyword, string projectCode)
        {
            return v_service.GetListNoPage(keyword, projectCode,null);
        }
        /// <summary>
        /// 根据项目编号获取宿舍信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<V_LodgingHouseEntity> GetLoadginHouse(string projectId)
        {
            return v_service.GetListNoPage(null, null, projectId);
        }
        /// <summary>
        /// 根据ID查询宿舍信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public V_LodgingHouseEntity GetFormById(string keyword)
        {
            return v_service.FindEntity(keyword);
        }
        /// <summary>
        /// 根据宿舍号查询宿舍信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<LodgingHouseEntity> GetEntityByCode(string code)
        {
            return service.FindByCode(code);
        }

        /// <summary>
        /// 添加或修改宿舍信息
        /// </summary>
        /// <param name="lodgingHouseEntity"></param>
        /// <param name="keyValue"></param>
        public void SubmitForm(LodgingHouseEntity lodgingHouseEntity, string keyValue)
        {
            if ("&nbsp".Equals(lodgingHouseEntity.F_HouseProvince))
            {
                lodgingHouseEntity.F_HouseProvince = null;
            }
            if ("&nbsp".Equals(lodgingHouseEntity.F_HouseCity))
            {
                lodgingHouseEntity.F_HouseCity = null;
            }
            if ("&nbsp".Equals(lodgingHouseEntity.F_HouseManage))
            {
                lodgingHouseEntity.F_HouseManage = null;
            }
            if ("&nbsp".Equals(lodgingHouseEntity.F_Description))
            {
                lodgingHouseEntity.F_Description = null;
            }
            if ("&nbsp".Equals(lodgingHouseEntity.F_DetailsAddress))
            {
                lodgingHouseEntity.F_DetailsAddress = null;
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                lodgingHouseEntity.Modify(keyValue);
            }
            else
            {
                lodgingHouseEntity.F_EnabledMark = true;
                lodgingHouseEntity.Create();
            }
            service.SubmitForm(lodgingHouseEntity, keyValue);
        }
        /// <summary>
        /// 多条插入
        /// </summary>
        /// <param name="list"></param>
        public void insertList(List<LodgingHouseEntity> list)
        {
            service.Insert(list);
        }
        /// <summary>
        /// 宿舍信息去重
        /// </summary>
        /// <param name="users"></param>
        public void DeleteDuplicate(List<string> houseCodes)
        {
            List<LodgingHouseEntity> list = new List<LodgingHouseEntity>();

            for (var j = 0; j < houseCodes.Count; j++)
            {
                list = service.FindByCode(houseCodes[j].ToString());
                for (var i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].F_HouseCode.ToString().Equals(list[i + 1].F_HouseCode.ToString()))
                    {
                        Delete(list[i + 1].F_Id);
                    }
                }
            }

        }
        /// <summary>
        /// 删除属蛇
        /// </summary>
        /// <param name="keyValue"></param>
        public void Delete(string keyValue)
        {
            LodgingHouseEntity lodgingHouseEntity = service.FindEntity(keyValue);
            if (lodgingHouseEntity != null)
            {
                lodgingHouseEntity.F_EnabledMark = false;//0无效
                service.Update(lodgingHouseEntity);
            }
            
        }
    }
}
