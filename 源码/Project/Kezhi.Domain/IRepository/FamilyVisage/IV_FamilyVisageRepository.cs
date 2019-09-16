using Kezhi.Data;
using Kezhi.Domain.Entity.FamilyVisage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.IRepository.FamilyVisage
{
    public interface IV_FamilyVisageRepository : IRepositoryBase<V_OA_FamilyVisageEntity>
    {
        List<V_OA_FamilyVisageEntity> GetListByOrganize(string organize,string role);
        int GetSort(string department, string userId);
        List<V_OA_FamilyVisageEntity> GetFamilyByDepartment(string department);

        List<V_OA_FamilyVisageEntity> GetFamilyByUserAndDepartment(string department, string roleName);
    }
}
