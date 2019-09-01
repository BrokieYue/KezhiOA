using System;


namespace Kezhi.Domain.Entity.OA
{
    /// <summary>
    /// 次实体只为修改工作日志所用
    /// </summary>
    public class V_WorkDailyRecordEntityUpdate
    {
        public string F_Id { get; set; }
        public string F_WorkUserName { get; set; }
        public string F_ItemName { get; set; }
        public DateTime? F_WorkDate { get; set; }
        public string F_ProjectName { get; set; }
        public string F_WorkAddressFirst { get; set; }
        public string F_WorkAddressFirst1 { get; set; }
        public string F_DailyRecord { get; set; }
        public string F_WorkUserId { get; set; }
        public string F_ProjectId { get; set; }
        public string F_WorkAddress { get; set; }
        public bool? F_IfLocal { get; set; }
        public string F_WorkType { get; set; }
        public string F_WorkTimeStart { get; set; }
        public string F_WorkTimeEnd { get; set; }
        public string F_WorkAddressCode { get; set; }
        public bool? F_CreatType { get; set; }
        public string F_Description { get; set; }
        public string F_ProjectCode { get; set; }
        public string F_WorkCategory { get; set; }
        public string F_OtherAddress { get; set; }
        public int? F_WorkSubsidy { get; set; }
       
    }
}
