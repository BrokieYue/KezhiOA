/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using Kezhi.Code;
using Kezhi.Data;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;
using System.Collections.Generic;
using System.Text;

namespace Kezhi.Repository.SystemManage
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<UserEntity>(t => t.F_Id == keyValue);
                db.Delete<UserLogOnEntity>(t => t.F_UserId == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(userEntity);
                }
                else
                {
                    userLogOnEntity.F_Id = userEntity.F_Id;
                    userLogOnEntity.F_UserId = userEntity.F_Id;
                    userLogOnEntity.F_UserSecretkey = Md5.md5(Common.CreateNo(), 16).ToLower();
                    userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userLogOnEntity.F_UserPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    db.Insert(userEntity);
                    db.Insert(userLogOnEntity);
                }
                db.Commit();
            }
        }


        public List<UserEntity> GetUserByDepartment(string department)
        {
            List<UserEntity> list = new List<UserEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT a.* FROM [KezhiOADB].[dbo].[Sys_User] as a 
	                        left join [KezhiOADB].[dbo].[Sys_Organize] as b
	                        on a.F_DepartmentId = b.F_Id 
	                        left join [KezhiOADB].[dbo].[T_OA_FamilyVisage] as c
	                        on a.F_Id = c.F_WorkUserId
                        where  a.F_RoleId is not null and c.F_WorkUserId is null and a.F_EnabledMark = 1 ");
            if (!string.IsNullOrEmpty(department) && !department.Equals(" "))
            {
                strSql.Append(@"  and b.F_FullName = '" + department + "'");
            }

            strSql.Append(@" order by a.F_IsAdministrator desc");

            return this.FindList(strSql.ToString()); ;
        }
    }
}
