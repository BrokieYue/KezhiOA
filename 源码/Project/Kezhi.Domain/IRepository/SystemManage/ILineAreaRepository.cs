using Kezhi.Data;
using Kezhi.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Kezhi.Domain.IRepository.SystemManage
{
    public interface ILineAreaRepository : IRepositoryBase<LineAreaEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(LineAreaEntity lineAreaEntity, string keyValue);
    }
}
