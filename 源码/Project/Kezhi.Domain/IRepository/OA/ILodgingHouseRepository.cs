using Kezhi.Data;
using Kezhi.Domain.Entity.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.IRepository.OA
{
    public interface ILodgingHouseRepository : IRepositoryBase<LodgingHouseEntity>
    {
        void SubmitForm(LodgingHouseEntity lodgingHouseEntity, string keyValue);

        List<LodgingHouseEntity> FindByCode(string code);
    }
}
