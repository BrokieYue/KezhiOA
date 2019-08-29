using Kezhi.Data;
using Kezhi.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Kezhi.Domain.IRepository.SystemManage
{
    public interface IStationInfoRepository : IRepositoryBase<StationInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(StationInfoEntity stationInfoEntity, string keyValue);
    }
}
