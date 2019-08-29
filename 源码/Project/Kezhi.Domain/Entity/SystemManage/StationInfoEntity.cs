using System;

namespace Kezhi.Domain.Entity.SystemManage
{
    public class StationInfoEntity : IEntity<StationInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string AreaCode { get; set; }//区域编码
        public string AreaName { get; set; }//区域名称
        public string StationCode { get; set; }//工位编码
        public string StationName { get; set; }//工位名称
        public string StationDesc { get; set; }//工位描述
        public int? StationNo { get; set; }//工位编号
        public int? StationType { get; set; }//工位类型:0-手动工位;1-自动工位;2-上件;3-质量采集点;4-返修工位;5-上线工位
        public int? OnViewPanel { get; set; }//上件看板数量
        public int? StationNum { get; set; }//序号
        public int? RobotCount { get; set; }//机器人数量
        public string PLCCode { get; set; }//PLC编码
        public string F_Description { get; set; }//备注
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
