using Kezhi.Application.SystemManage;
/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Code;
using Kezhi.Domain.Entity.OA;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.OA;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.OA;
using Kezhi.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kezhi.Application.OAManage
{
    public class ProjectApp
    {
        private IV_ProjectRepository service = new V_ProjectRepository();
        private IProjectRepository service1 = new ProjectRepository();

        /// <summary>
        /// 获取所有项目列表
        /// </summary>
        /// <returns></returns>
        public List<ProjectEntity> GetList()
        {
            return service1.IQueryable().ToList();
        }
        /// <summary>
        /// 根据项目编号或项目名称模糊查询获取项目列表，带分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<V_ProjectEntity> GetList(Pagination pagination, string keyword, string projectStatus,string projectType)
        {
            var expression = ExtLinq.True<V_ProjectEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_ProjectCode.Equals(keyword));
                expression = expression.Or(t => t.F_ProjectName.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(projectStatus))
            {
                if (projectStatus.Equals("暂无状态"))
                {
                    expression = expression.And(t => t.F_ProjectStatus.Equals(projectStatus));
                    expression = expression.Or(t => t.F_ProjectStatus == null);
                }
                else
                {
                    expression = expression.And(t => t.F_ProjectStatus.Equals(projectStatus));
                }
                
            }
            if (!string.IsNullOrEmpty(projectType))
            {
                expression = expression.And(t => t.F_ProjectType.Equals(projectType));
            }
            return service.FindList(expression, pagination);
        }
        /// <summary>
        /// 不带分页查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<V_ProjectEntity> GetLists(string keyword,string projectStatus,string projectType)
        {
            return service.GetListsNoPage(keyword, projectStatus, projectType);
        }
        /// <summary>
        /// 根据项目编号或项目名称模糊查获取项目列表，不带分页
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<V_ProjectEntity> GetList(string keyword)
        {

            return GetList(null,keyword,null,null);
        }
        public List<ProjectEntity> GetListByStatus(string[] status,string parentId)
        {
            return service1.GetListByStatus(status, parentId);
        }
        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ProjectEntity GetForm(string keyValue)
        {
            return service1.FindEntity(keyValue);
        }
        public V_ProjectEntity GetFormById(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        /// <summary>
        /// 根据主键删除对象
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteForm(string keyValue)
        {
            service1.DeleteForm(keyValue);
        }
        /// <summary>
        /// 提交新增/修改数据到数据库
        /// </summary>
        /// <param name="projectEntity"></param>
        /// <param name="keyValue"></param>
        public void SubmitForm(ProjectEntity projectEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                projectEntity.Modify(keyValue);
            }
            else
            {
                projectEntity.Create();
            }
            service1.SubmitForm(projectEntity, keyValue);
        }
        /// <summary>
        /// 项目编号精确获取对象
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public ProjectEntity GetProject(string projectCode)
        {
            ProjectEntity projectEntity = service1.FindEntity(t => t.F_ProjectCode.Equals(projectCode));
            return projectEntity;
        }

        public void InsertList(List<ProjectEntity> list)
        {
            service1.Insert(list);

        }
        /// <summary>
        /// 项目去重
        /// </summary>
        /// <param name="users"></param>
        public void DeleteDuplicate()
        {
            List<ProjectEntity> list = new List<ProjectEntity>();
            list = service1.GetListOrderByDate();
            for (var i = 0; i < list.Count - 2; i++)
            {
                if (list[i].F_ProjectCode.ToString().Equals(list[i + 1].F_ProjectCode.ToString()) && list[i].F_ProjectName.Equals(list[i + 1].F_ProjectName))
                {
                    service1.Delete(list[i + 1]);
                }
            }

        }


    }
}
