using Kezhi.Data;
using Kezhi.Domain.Entity.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.IRepository.OA
{
    public interface IV_LodgingHouseRepository : IRepositoryBase<V_LodgingHouseEntity>
    {
        List<V_LodgingHouseEntity> GetListNoPage(string keyword, string projectCode, string projectId);
    }
}
