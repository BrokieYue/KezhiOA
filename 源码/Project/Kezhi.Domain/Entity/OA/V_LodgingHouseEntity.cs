using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.Entity.OA
{
    public class V_LodgingHouseEntity 
    {
     public string F_Id{get;set;}
    public string F_HouseCode{get;set;}
    public string F_HouseName{get;set;}
    public string F_ProjectId{get;set;}
    public string F_ProjectName { get; set; }
    public string F_HouseManage{get;set;}
    public string F_HouseManageName { get; set; }
    public string F_HouseProvince{get;set;}
    public string F_HouseCity{get;set;}
    public string F_DetailsAddress{get;set;}
    public string F_HouseAddress { get; set; }


    public string F_Description{get;set;}
    public bool? F_EnabledMark{get;set;}
    public bool? F_DeleteMark{get;set;}
    public DateTime? F_CreatorTime{get;set;}
    public string F_CreatorUserId{get;set;}
    public DateTime? F_LastModifyTime{get;set;}
    public string F_LastModifyUserId{get;set;}
    public DateTime? F_DeleteTime{get;set;}
    public string F_DeleteUserId { get; set; }

    }
}
