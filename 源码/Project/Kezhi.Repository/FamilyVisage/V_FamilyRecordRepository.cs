using Kezhi.Data;
using Kezhi.Data.Extensions;
using Kezhi.Domain.Entity.FamilyVisage;
using Kezhi.Domain.IRepository.FamilyVisage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Repository.FamilyVisage
{
    public class V_FamilyRecordRepository : RepositoryBase<V_OA_FamilyVisageEntity>, IV_FamilyVisageRepository
    {
        public List<V_OA_FamilyVisageEntity> GetListByOrganize(string organize,string role)
        {
            List<V_OA_FamilyVisageEntity> list = new List<V_OA_FamilyVisageEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM [KezhiOADB].[dbo].[V_OA_FamilyVisage] where 1= 1 ");
            if (!string.IsNullOrEmpty(organize) && !organize.Equals(" "))
            {
                strSql.Append(@"  and F_OrganizeName = '" + organize + "'");
            }
            if (!string.IsNullOrEmpty(role) && !role.Equals(" ") && role.Equals("员工"))
            {
                strSql.Append(@"and F_RoleName =  '员工'");
            }
            else if (!string.IsNullOrEmpty(role) && !role.Equals(" ") && !role.Equals("员工"))
            {
                strSql.Append(@"and F_RoleName !=  '员工'");

            }
            strSql.Append(@" order by F_Sort asc");

            return this.FindList(strSql.ToString()); 
        }

        public int GetSort(string department, string roleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT case when max(F_Sort) is null then 1 else max(F_Sort) + 1 end as F_Sort
                      FROM [KezhiOADB].[dbo].[V_OA_FamilyVisage]  where 1 = 1");
            if (!string.IsNullOrEmpty(department) && !department.Equals(" "))
            {
                strSql.Append(@"  and F_OrganizeName = '" + department + "'");
            }
            if (!string.IsNullOrEmpty(roleName) && !roleName.Equals(" "))
            {
                if (roleName.Equals("员工"))
                {
                    strSql.Append(@" and F_RoleName = '员工'");
                }
                else
                {
                    strSql.Append(@" and F_RoleName != '员工'");
                }
            }
            DataTable dt = DbHelper.GetDataTable(strSql.ToString());
            int result = Convert.ToInt32(dt.Rows[0][0].ToString());
            return result;
        }

        public List<V_OA_FamilyVisageEntity> GetFamilyByDepartment(string department)
        {
            List<V_OA_FamilyVisageEntity> list = new List<V_OA_FamilyVisageEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"  select * from [KezhiOADB].[dbo].[V_OA_FamilyVisage] where 1 = 1 ");
            if (!string.IsNullOrEmpty(department) && !department.Equals(" "))
            {
                strSql.Append(@"  and F_OrganizeName =  '" + department + "'");
            }
            strSql.Append(@" order by F_Sort asc");

            return this.FindList(strSql.ToString()); 
        }

        public List<V_OA_FamilyVisageEntity> GetFamilyByUserAndDepartment(string department, string userId)
        {
            List<V_OA_FamilyVisageEntity> list = new List<V_OA_FamilyVisageEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"  select * from [KezhiOADB].[dbo].[V_OA_FamilyVisage] where 1 = 1 ");
            if (!string.IsNullOrEmpty(department) && !department.Equals(" "))
            {
                strSql.Append(@"  and F_OrganizeName =  '" + department + "'");
            }
            if (!string.IsNullOrEmpty(userId) && !userId.Equals(" "))
            {
                strSql.Append(@"  and F_WorkUserId =  '" + userId + "'");
            }
            strSql.Append(@" order by F_Sort asc");

            var data =  this.FindList(strSql.ToString());
            return data;
        }
    }
}
