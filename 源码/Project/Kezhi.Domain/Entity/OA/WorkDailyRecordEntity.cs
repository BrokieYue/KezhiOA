using System;


namespace Kezhi.Domain.Entity.OA
{
    public class WorkDailyRecordEntity : IEntity<WorkDailyRecordEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_WorkDate { get; set; }
        public string F_DailyRecord { get; set; }
        public string F_WorkUserId { get; set; }
        public string F_ProjectId { get; set; }
        public string F_WorkAddress { get; set; }
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
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public bool? F_CurrentDayMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public string F_WorkCategory{get;set;}
        public string F_OtherAddress { get; set; }

        public string F_LodgingHouse { get; set; }

    }
}
