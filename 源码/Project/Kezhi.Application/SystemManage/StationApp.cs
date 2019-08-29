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
    public class StationApp
    {
        private IStationRepository service = new StationRepository();
        public List<StationEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<StationEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.StationCode.Contains(keyword));
                expression = expression.Or(t => t.StationName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public List<StationEntity> GetList1()
        {
            return service.IQueryable().OrderBy(t => t.F_Id).ToList();
        }

        public List<StationEntity> GetStationListBySql(string F_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from UDT_PM_Station where F_Id=@F_Id");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@F_Id",F_ID)
            };
            return service.FindList(strSql.ToString(), parameter);
        }
        public DataTable GetStationListBySqldt(string keyword)
        {
            SQLCommon SqlDeal = new SQLCommon();
            string QuerySql=@"select * from UDT_PM_Station where 1=1";
            if (!string.IsNullOrEmpty(keyword))
            {
                QuerySql = QuerySql + " And (StationName like '%" + keyword + "%' or StationCode like '%" + keyword + "%')";
            }
            DataTable dtStation = SqlDeal.GetDataTable(QuerySql);
            return dtStation;
        }
        public List<StationEntity> GetStationListByProc(string F_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Exec sp_local_getStation @F_Id");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@F_Id",F_Id)
            };
            return service.FindList(strSql.ToString(), parameter);
        }

        public List<StationEntity> GetList2()
        {
            return service.IQueryable().OrderBy(t => t.F_Id).ToList();
        }
        public StationEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(StationEntity stationEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                stationEntity.Modify(keyValue);
                service.Update(stationEntity);
            }
            else
            {
                stationEntity.Create();
                service.Insert(stationEntity);
            }
        }
    }
}
