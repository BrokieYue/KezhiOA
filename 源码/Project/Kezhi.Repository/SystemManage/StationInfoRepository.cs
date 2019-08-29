using Kezhi.Code;
using Kezhi.Data;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;

namespace Kezhi.Repository.SystemManage
{
    public class StationInfoRepository : RepositoryBase<StationInfoEntity>, IStationInfoRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<StationInfoEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(StationInfoEntity stationInfoEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(stationInfoEntity);
                }
                else
                {
                    db.Insert(stationInfoEntity);
                }
                db.Commit();
            }
        }
    }
}
