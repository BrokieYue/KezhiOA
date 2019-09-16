using System;


namespace Kezhi.Domain.Entity.OA
{
    public class V_ProjectEntity : IEntity<V_ProjectEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ProjectCode { get; set; }
        public string F_ProjectName { get; set; }
        public string F_ClientContractNO { get; set; }
        public string F_ProjectAddress { get; set; }
        public string F_ProjectProvence { get; set; }
        public string F_ProjectCity { get; set; }
        public string F_ProjectManagerId { get; set; }
        public string F_ProjectType { get; set; }
        public DateTime? F_ProjectTimeStart { get; set; }
        public DateTime? F_ProjectTimeEnd { get; set; }
        public string F_ProjectClient { get; set; }
        public string F_ProjectStatus { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public string F_ProjectManagerName { get; set; }
        public string F_CreateorUserName { get; set; }
        public string F_LastModifyUserName { get; set; }
        public string F_DeleteUserName { get; set; }
    }
}
