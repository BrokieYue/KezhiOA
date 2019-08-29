using Kezhi.Data;
using Kezhi.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Kezhi.Domain.IRepository.SystemManage
{
    public interface IStationRepository : IRepositoryBase<StationEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(StationEntity stationEntity, string keyValue);
    }
}
