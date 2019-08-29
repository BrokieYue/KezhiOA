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
    public class WorkDailyRecordApp
    {
        private IV_WorkDailyRecordRepository service = new V_WorkDailyRecordRepository();
        private IWorkDailyRecordRepository service1 = new WorkDailyRecordRepository();
        private UserApp userApp = new UserApp();
        private RoleApp roleApp = new RoleApp();
        

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns></returns>
        public List<V_WorkDailyRecordEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        /// <summary>
        /// 根据员工ID或员工姓名模糊查询员工对象列表，带分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<V_WorkDailyRecordEntity> GetList(Pagination pagination, string keyword, DateTime? startTime, DateTime? endTime, string organize,string filiale)
        {
            var expression = ExtLinq.True<V_WorkDailyRecordEntity>();
            var LoginInfo = OperatorProvider.Provider.GetCurrent();

            DateTime currentMounth = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            //根据登录的用户查询自己的工作日志：如果岗位是员工，只支持查询自己的工作日志
            string role = "";
            if (!LoginInfo.UserCode.Equals("admin"))
            {
                role = roleApp.GetForm(userApp.GetForm(LoginInfo.UserId).F_DutyId).F_FullName;
                if (!string.IsNullOrEmpty(role) && role.Equals("员工"))
                {
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        UserEntity user = userApp.GetUser(keyword);
                        if (user != null)
                        {
                            expression = expression.And(t => t.F_WorkUserId.Equals(user.F_Id));
                        }
                        else
                        {
                            expression = expression.And(t => t.F_WorkUserId.Equals(""));
                        }
                    }
                    if(startTime != null && endTime != null)
                    {
                        expression = expression.And(t => t.F_WorkDate >= startTime);
                        expression = expression.And(t => t.F_WorkDate <= endTime);
                    }
                    if (startTime == null && endTime == null)
                    {
                        expression = expression.And(t => t.F_WorkDate >= currentMounth);
                    }
                    expression = expression.And(t => t.F_WorkUserId.Equals(LoginInfo.UserId));
                }
                else
                {
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        UserEntity user = userApp.GetUser(keyword);
                        if (user != null)
                        {
                            expression = expression.And(t => t.F_WorkUserId.Equals(user.F_Id));
                        }
                        else
                        {
                            expression = expression.And(t => t.F_WorkUserId.Equals(""));
                        }
                    }
                    if (startTime != null && endTime != null)
                    {
                        expression = expression.And(t => t.F_WorkDate >= startTime);
                        expression = expression.And(t => t.F_WorkDate <= endTime);
                    }
                    if (!string.IsNullOrEmpty(organize))
                    {
                        string orgainzename = organize.Trim();
                        expression = expression.And(t => t.F_OrganizeName.Equals(orgainzename));
                    }
                    if (startTime == null && endTime == null)
                    {
                        expression = expression.And(t => t.F_WorkDate >= currentMounth);
                    }
                    if (!string.IsNullOrEmpty(filiale))
                    {
                        expression = expression.And(t => t.F_ItemId.Equals(filiale));
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    UserEntity user = userApp.GetUser(keyword);
                    if (user != null)
                    {
                        expression = expression.And(t => t.F_WorkUserId.Equals(user.F_Id));
                    }
                    else
                    {
                        expression = expression.And(t => t.F_WorkUserId.Equals(""));

                    }
                }
                if (startTime != null && endTime != null)
                {
                    expression = expression.And(t => t.F_WorkDate >= startTime);
                    expression = expression.And(t => t.F_WorkDate <= endTime);
                }
                if (!string.IsNullOrEmpty(organize))
                {
                    string orgainzename = organize.Trim();
                    expression = expression.And(t => t.F_OrganizeName.Equals(orgainzename));
                }
               if (startTime == null && endTime == null)
                {
                    expression = expression.And(t => t.F_WorkDate >= currentMounth);
                }
               if (!string.IsNullOrEmpty(filiale))
               {
                   expression = expression.And(t => t.F_ItemId.Equals(filiale));
               }
            }
           
            return service.FindList(expression, pagination);
        }

        /// <summary>
        /// 根据员工ID或员工姓名模糊查询员工对象列表，不带分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<V_WorkDailyRecordEntity> GetListNoPage(string keyword, DateTime? startTime, DateTime? endTime ,string organize, string filiale)
        {

            return service.GetLists(keyword, startTime, endTime, organize, filiale);
        }
      
        /// <summary>
        /// 导出：查询周报
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="organize"></param>
        /// <param name="filiale"></param>
        /// <returns></returns>
        public List<V_WorkDailyRecordEntity> GetWeekListNoPage(string keyword, string projectCode, DateTime? startTime, DateTime? endTime, string organize, string filiale)
        {

            return service.GetWeekLists(keyword, projectCode, startTime, endTime, organize, filiale);
        }

        public List<V_WorkDailyRecordEntity> GetListByUserAndProject(string user, string project,DateTime? startTime,DateTime? endTime)
        {

            return service.GetListByUserAndProject(user, project,startTime,endTime);
        }

        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public V_WorkDailyRecordEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public WorkDailyRecordEntity GetEntity(string keyValue)
        {
            return service1.FindEntity(keyValue);
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
        /// <param name="workDailyRecordEntity"></param>
        /// <param name="keyValue"></param>
        public void SubmitForm(WorkDailyRecordEntity workDailyRecordEntity, string keyValue)
        {

            if (!string.IsNullOrEmpty(keyValue))
            {
                workDailyRecordEntity.Modify(keyValue);
            }
            else
            {
                workDailyRecordEntity.Create();
            }

            service1.SubmitForm(workDailyRecordEntity, keyValue);
        }

        public void InsertList(List<WorkDailyRecordEntity> list)
        {
            service1.Insert(list);
            
        }

        public void DeleteDuplicate(List<string> users)
        {
            List<WorkDailyRecordEntity> list = new List<WorkDailyRecordEntity>();
          
            for (var j = 0; j < users.Count; j++)
            {
                list = service1.FindListByUserAndProject(users[j]);
                for (var i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].F_WorkDate.ToString().Equals(list[i + 1].F_WorkDate.ToString()) && list[i].F_WorkUserId.ToString().Equals(list[i + 1].F_WorkUserId.ToString()))
                    {
                        service1.Delete(list[i + 1]);
                    }
                }
            }
              
        }

        public bool GetWorkDailyByWorkDate(string workDate)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            var userId = LoginInfo.UserId.ToString();
            List<WorkDailyRecordEntity> list = service1.GetWorkDailyByWorkDate(userId,workDate);
            if (list != null && list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
