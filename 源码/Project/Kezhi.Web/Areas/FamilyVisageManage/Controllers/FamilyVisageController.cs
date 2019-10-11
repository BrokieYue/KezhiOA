using Kezhi.Application.FamilyVisage;
using System;
using Kezhi.Code;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using Kezhi.Domain.Entity.FamilyVisage;
using Kezhi.Application.SystemManage;
using Kezhi.Domain.Entity.SystemManage;

namespace Kezhi.Web.Areas.FamilyVisageManage.Controllers
{
    public class FamilyVisageController : ControllerBase
    {
        private FamilyVisageApp familyApp = new FamilyVisageApp();
        private UserApp userApp = new UserApp();
        private RoleApp roleApp = new RoleApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string organize,string role)
        {
            var data = familyApp.GetListByOrganize(organize, role);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSrot(string department, string userId)
        {
            string roleName = getRole(userId).F_FullName;
            var data = familyApp.GetSort(department, roleName);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(T_OA_FamilyVisageEntityPlus entity)
        {
            T_OA_FamilyVisageEntity familyEntity = new T_OA_FamilyVisageEntity();
            familyEntity.F_WorkUserId = entity.F_WorkUserId;
            familyEntity.F_Valuation = entity.F_Valuation;
            familyEntity.F_Sort = entity.F_Sort;
            familyEntity.F_Position = entity.F_Position;
            familyEntity.F_PhotoUrl = entity.F_PhotoUrl;
            familyEntity.F_Description = entity.F_Description;
            //执行员工风采制作
            if (!string.IsNullOrEmpty(entity.ImageWidth))
            {
                string[] strText = null;
                if (entity.F_Valuation.Contains(','))
                {
                    strText = entity.F_Valuation.Split(',');
                }
                else
                {
                    strText = entity.F_Valuation.Split('，');
                }
                string pictureImg = PictureUtil.PhotoAssembleyIT(strText,entity.ImageWidth, entity.ImageHeight, entity.F_PhotoUrl, "ITBackImage.png");
                if (string.IsNullOrEmpty(pictureImg))
                {
                    return Error("员工风采制作出现错误，请重新制作或联系开发者！");
                }
                else
                {
                    familyEntity.F_PictureUrl = pictureImg;
                }
            }
            else
            {
                return Error("员工照片有误，请重新上传");
            }
            familyApp.SubmitForm(familyEntity, null);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Update(T_OA_FamilyVisageEntity entity, string keyValue)
        {

            familyApp.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string keyValue)
        {
          
            familyApp.DeleteForm(keyValue);
         
            return Success("操作成功。");
        }
        /// <summary>
        /// 获取该部门已添加员工风采的员工
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFamilyByDepartment(string department)
        {
            var data = familyApp.GetFamilyByDepartment(department);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取单个员工风采
        /// </summary>
        /// <param name="department"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFamilyByUserAndDepartment(string department, string userId)
        {
            var data = familyApp.GetFamilyByUserAndDepartment(department,userId);
            return Content(data.ToJson());
        }
        public ActionResult IT()
        {
            return View();
        }
        public ActionResult UpdateForm()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }
        /// <summary>
        /// 根据用户ID查询岗位
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public RoleEntity getRole(string userId)
        {
            UserEntity user = userApp.GetForm(userId);
            return roleApp.GetForm(user.F_DutyId);
        }
    }
}