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
    public class LodgingHouseRepository: RepositoryBase<LodgingHouseEntity>, ILodgingHouseRepository
    {
        public void SubmitForm(LodgingHouseEntity lodgingHouseEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(lodgingHouseEntity);
                }
                else
                {
                    db.Insert(lodgingHouseEntity);
                }
                db.Commit();
            }
        }

        public List<LodgingHouseEntity> FindByCode(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM [KezhiOADB].[dbo].[T_OA_LodgingHouse] where F_EnabledMark = 1 and F_HouseCode = '" + code + "' order by F_CreatorTime desc");

            return this.FindList(strSql.ToString()); 
        }


        public List<LodgingHouseEntity> GetListNoPage(string keyword)
        {
            List<LodgingHouseEntity> list = new List<LodgingHouseEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"  SELECT * FROM [KezhiOADB].[dbo].[T_OA_LodgingHouse] where F_EnabledMark = 1 ");
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@"and (F_HouseCode like '%" + keyword + "%' or F_HouseName like '%" + keyword + "%')");
            }
            strSql.Append(@" order by F_CreatorTime desc");

            return this.FindList(strSql.ToString()); ;
        }
    }
}
