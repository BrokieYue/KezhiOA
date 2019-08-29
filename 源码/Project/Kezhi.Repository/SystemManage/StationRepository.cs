using Kezhi.Code;
using Kezhi.Data;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;

namespace Kezhi.Repository.SystemManage
{
    public class StationRepository : RepositoryBase<StationEntity>, IStationRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<StationEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(StationEntity stationEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(stationEntity);
                }
                else
                {
                    db.Insert(stationEntity);
                }
                db.Commit();
            }
        }
    }
}
