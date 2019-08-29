using System;

namespace Kezhi.Domain.Entity.SystemManage
{
    public class LineAreaEntity : IEntity<LineAreaEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string FactoryCode { get; set; }//工厂代码
        public string LineCode { get; set; }//生产线
        public string ProductionTechType { get; set; }//工艺类型
        public string AreaCode { get; set; }//区域编码
        public string AreaName { get; set; }//区域名称
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}
