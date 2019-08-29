using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.Entity.OA
{
    public class JobPerformanceEntitiy
    {
        public string F_Id { get; set; }
        public string F_WorkUserName { get; set; }
        public DateTime? F_WorkDate { get; set; }
        public string F_WorkUserId { get; set; }
        public string F_ProjectId { get; set; }
        public string F_WorkAddress { get; set; }
        public string F_WorkAddressCode { get; set; }
        public string F_WorkedHours { get; set; }
        public string F_PayHours { get; set; }
        public string F_RestHours { get; set; }
        public string F_DeductHours { get; set; }
        public string F_MealSubsidy { get; set; }
        public string F_WorkSubsidy { get; set; }
        public string F_ProjectCode { get; set; }

    }
}
