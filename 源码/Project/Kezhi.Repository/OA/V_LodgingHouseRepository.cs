using Kezhi.Data;
using Kezhi.Domain.Entity.OA;
using Kezhi.Domain.IRepository.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Repository.OA
{
    public class V_LodgingHouseRepository : RepositoryBase<V_LodgingHouseEntity>, IV_LodgingHouseRepository
    {
        public List<V_LodgingHouseEntity> GetListNoPage(string keyword, string projectCode,string projectId)
        {
            List<V_LodgingHouseEntity> list = new List<V_LodgingHouseEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"  SELECT * FROM [KezhiOADB].[dbo].[V_LodgingHouse] where F_EnabledMark = 1 ");
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@"and (F_HouseCode like '%" + keyword + "%' or F_HouseName like '%" + keyword + "%')");
            }
            if (!string.IsNullOrEmpty(projectCode))
            {
                strSql.Append(@"and (F_ProjectId like '%" + projectCode + "%' or F_ProjectName like '%" + projectCode + "%')");
            }
            if (!string.IsNullOrEmpty(projectId))
            {
                strSql.Append(@"and F_ProjectId = '" + projectId + "'");
            }
            strSql.Append(@" order by F_ProjectId desc");

            return this.FindList(strSql.ToString()); ;
        }
    }
}
