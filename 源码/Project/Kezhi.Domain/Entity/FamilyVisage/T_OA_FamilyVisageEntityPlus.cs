using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Domain.Entity.FamilyVisage
{
    /// <summary>
    /// 此实体只为传递照片的宽高
    /// </summary>
    public class T_OA_FamilyVisageEntityPlus
    {
        public string F_Id {get;set;}
        public string F_WorkUserId{get;set;}
        public string F_Position{get;set;}
        public string F_Valuation{get;set;}
        public string F_PhotoUrl{get;set;}
        public string F_PictureUrl{get;set;}
        public int? F_Sort { get; set; }
        public string F_Description { get; set; }
        public string ImageWidth { get; set; }
        public string ImageHeight { get; set; }

    }
}
