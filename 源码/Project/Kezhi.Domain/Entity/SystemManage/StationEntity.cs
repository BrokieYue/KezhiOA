using System;

namespace Kezhi.Domain.Entity.SystemManage
{
    public class StationEntity : IEntity<StationEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string StationCode { get; set; }//工位编号
        public string LineCode { get; set; }//区域线体
        public string StationName { get; set; }//工位名称
        public string StationDesc { get; set; }//工位描述
        public int StationLocation { get; set; }//工位位置:1-缓存区;2-线边
        public int StationType { get; set; }//工位类型:0-普通工位;1-检验工位;2-返修工位
        public string StationAbridge { get; set; }//工位缩写
        public int StationNum { get; set; }//工位序号
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
