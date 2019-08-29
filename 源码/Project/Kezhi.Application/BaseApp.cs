using Kezhi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Application
{
    public class BaseApp
    {
        /// <summary>
        /// 提交接口返回结果-chenchao
        /// </summary>
        /// <param name="ErrorType">接口类型</param>
        /// <param name="mesError">Mes系统是否异常:0异常;1正常</param>
        /// <param name="sapError">外部接口是否返回成功:0失败;1成功</param>
        /// <param name="Message">Mes系统异常或错误信息</param>
        /// <param name="Spare_Text1">外部接口返回状态码</param>
        /// <param name="Spare_Text2">外部接口返回提示信息</param>
        /// <param name="Spare_Text3">预留字段（暂存业务编号）</param>
        public void InsertErrorLog(string ErrorType, int mesError, int sapError, string Message, string Spare_Text1, string Spare_Text2, string Spare_Text3)
        {
            SQLCommon SqlDeal = new SQLCommon();
            string strSql = string.Format(@"insert into  [dbo].[T_Interface_ErrorLog](F_Id,Error_Type,[MESSAGE],Spare_Text1,Spare_Text2,Spare_Text3,MESError,SAPError,F_CreatorTime) 
                                            values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')"
                                        , Guid.NewGuid().ToString(), ErrorType, Message, Spare_Text1, Spare_Text2, Spare_Text3, mesError, sapError, DateTime.Now.ToString());
            SqlDeal.ExecuteSQL(strSql);
        }
    }
}
