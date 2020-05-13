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
        public List<LodgingHouseEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<LodgingHouseEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_HouseCode.Contains(keyword));
                expression = expression.Or(t => t.F_HouseName.Contains(keyword));
            }
            expression = expression.And(t => t.F_EnabledMark == true);
            return service.FindList(expression, pagination);
        }
        /// <summary>
        /// 不分页查询
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public List<LodgingHouseEntity> GetLists(string keyword)
        {
            return service.GetListNoPage(keyword);
        }
 
        /// <summary>
        /// 根据ID查询宿舍信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public LodgingHouseEntity GetFormById(string keyword)
        {
            return service.FindEntity(keyword);
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
