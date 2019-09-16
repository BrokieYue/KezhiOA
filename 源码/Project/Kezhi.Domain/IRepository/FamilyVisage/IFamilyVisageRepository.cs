using Kezhi.Data;
using Kezhi.Domain.Entity.FamilyVisage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.IRepository.FamilyVisage
{
    public interface IFamilyVisageRepository : IRepositoryBase<T_OA_FamilyVisageEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(T_OA_FamilyVisageEntity familyVisageEntity, string keyValue);
    }
}
