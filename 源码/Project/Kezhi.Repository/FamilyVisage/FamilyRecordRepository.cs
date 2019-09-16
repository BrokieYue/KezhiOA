using Kezhi.Data;
using Kezhi.Domain.Entity.FamilyVisage;
using Kezhi.Domain.IRepository.FamilyVisage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Repository.FamilyVisage
{
    public class FamilyRecordRepository : RepositoryBase<T_OA_FamilyVisageEntity>, IFamilyVisageRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<T_OA_FamilyVisageEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(T_OA_FamilyVisageEntity familyVisageEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(familyVisageEntity);
                }
                else
                {
                    db.Insert(familyVisageEntity);
                }
                db.Commit();
            }
        }
    }
}
