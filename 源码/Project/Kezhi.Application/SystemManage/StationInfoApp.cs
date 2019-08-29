using Kezhi.Code;
/*******************************************************************************
 * Copyright © 2016 科致电气
 * Author: Kezhi
 * Description: 上海科致MES系统
 * Website：www.kezhi-electric.com
*********************************************************************************/
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.IRepository.SystemManage;
using Kezhi.Repository.SystemManage;
using Kezhi.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Kezhi.Application.SystemManage
{
    public class StationInfoApp
    {
        private IStationInfoRepository service = new StationInfoRepository();
        public List<StationInfoEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<StationInfoEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.StationCode.Contains(keyword));
                expression = expression.Or(t => t.StationName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public List<StationInfoEntity> GetList1()
        {
            return service.IQueryable().OrderBy(t => t.F_Id).ToList();
        }
   
        public List<StationInfoEntity> GetListByType(int keyValue)
        {
            string strSql = "select * from UDT_PM_StationInfo where 1=1";
            if (!string.IsNullOrEmpty(keyValue.ToString()))
            {
                strSql = strSql + " And StationType=" + keyValue.ToString();
            }
            return service.FindList(strSql);
        }

        public List<StationInfoEntity> GetStationListBySql(string F_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from UDT_PM_StationInfo where F_Id=@F_Id");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@F_Id",F_ID)
            };
            return service.FindList(strSql.ToString(), parameter);
        }
        public DataTable GetStationListBySqldt(string keyword)
        {
            SQLCommon SqlDeal = new SQLCommon();
            string QuerySql=@"select * from UDT_PM_StationInfo where 1=1";
            if (!string.IsNullOrEmpty(keyword))
            {
                QuerySql = QuerySql + " And (StationName like '%" + keyword + "%' or StationCode like '%"+keyword+"%' or AreaCode like '%"+keyword+"%')";
            }
            DataTable dtStation = SqlDeal.GetDataTable(QuerySql);
            return dtStation;
        }
        public List<StationInfoEntity> GetStationListByProc(string F_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Exec sp_local_getStation @F_Id");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@F_Id",F_Id)
            };
            return service.FindList(strSql.ToString(), parameter);
        }

        public List<StationInfoEntity> GetList2()
        {
            return service.IQueryable().OrderBy(t => t.F_Id).ToList();
        }
        public StationInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(StationInfoEntity stationInfoEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                stationInfoEntity.Modify(keyValue);
                service.Update(stationInfoEntity);
            }
            else
            {
                stationInfoEntity.Create();
                service.Insert(stationInfoEntity);
            }
        }
    }
}
