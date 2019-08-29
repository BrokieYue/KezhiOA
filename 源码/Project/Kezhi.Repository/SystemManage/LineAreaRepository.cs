using Kezhi.Code;
using Kezhi.Data;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;

namespace Kezhi.Repository.SystemManage
{
    public class LineAreaRepository : RepositoryBase<LineAreaEntity>, ILineAreaRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<LineAreaEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(LineAreaEntity lineAreaEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(lineAreaEntity);
                }
                else
                {
                    db.Insert(lineAreaEntity);
                }
                db.Commit();
            }
        }
    }
}
