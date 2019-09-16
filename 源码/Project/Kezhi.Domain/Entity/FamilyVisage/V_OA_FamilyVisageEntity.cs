using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.Entity.FamilyVisage
{
    public class V_OA_FamilyVisageEntity : IEntity<V_OA_FamilyVisageEntity>
    {
        public string F_Account { get; set; }
        public string F_RealName { get; set; }
        public bool? F_Enable { get; set; }
        public string F_OrganizeName { get; set; }
        public string F_RoleName { get; set; }
        public string F_Id {get;set;}
        public string F_WorkUserId{get;set;}
        public string F_Position{get;set;}
        public string F_Valuation { get; set; }
        public string F_PhotoUrl{get;set;}
        public string F_PictureUrl{get;set;}
        public int? F_Sort { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }

    }
}
