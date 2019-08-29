using System;


namespace Kezhi.Domain.Entity.OA
{
    /// <summary>
    /// 此实体只为新建日志时，接收其他工作地点，要是修改WorkDailyRecordEntity，会影响其他方法
    /// </summary>
    public class WorkDailyRecordEntityPlus : IEntity<WorkDailyRecordEntityPlus>
    {
        public string F_Id { get; set; }
        public DateTime? F_WorkDate { get; set; }
        public string F_DailyRecord { get; set; }
        public string F_WorkUserId { get; set; }
        public string F_ProjectId { get; set; }
        public string F_WorkAddress { get; set; }
        public string F_WorkAddressOther { get; set; }
        public bool? F_IfLocal { get; set; }
        public string F_WorkType { get; set; }
        public string F_WorkTimeStart { get; set; }
        public string F_WorkTimeEnd { get; set; }
        public string F_WorkAddressCode { get; set; }
        public string F_WorkedHours { get; set; }
        public string F_PayHours { get; set; }
        public string F_RestHours { get; set; }
        public string F_DeductHours { get; set; }
        public int? F_MealSubsidy { get; set; }
        public int? F_WorkSubsidy { get; set; }
        public bool? F_CreatType { get; set; }
        public string F_Description { get; set; }
  

    }
}
