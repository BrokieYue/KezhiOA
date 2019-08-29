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
    public class LineAreaApp
    {
        private ILineAreaRepository service = new LineAreaRepository();
        public List<LineAreaEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<LineAreaEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.AreaCode.Contains(keyword));
                expression = expression.Or(t => t.AreaName.Contains(keyword));
            }
            return service.FindList(expression, pagination);
        }

        public List<LineAreaEntity> GetList1()
        {
            return service.IQueryable().OrderBy(t => t.F_Id).ToList();
        }

        public LineAreaEntity GetListByCode(string keyword)
        {
            SQLCommon SqlDeal = new SQLCommon();
            string QuerySql = @"select * from UDT_PM_LineArea where AreaCode='" + keyword + "'";
            DataTable dtArea = SqlDeal.GetDataTable(QuerySql);
            LineAreaEntity AreaResult = new LineAreaEntity();
            if (dtArea.Rows.Count > 0)
            {
                AreaResult.AreaName = dtArea.Rows[0]["AreaName"].ToString();
            }
            return AreaResult;
        }

        public List<LineAreaEntity> GetAreaListBySql(string F_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from UDT_PM_LineArea where F_Id=@F_Id");
            DbParameter[] parameter = 
            {
                 new SqlParameter("@F_Id",F_ID)
            };
            return service.FindList(strSql.ToString(), parameter);
        }

        public LineAreaEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.F_Id == keyValue);
        }
        public void SubmitForm(LineAreaEntity lineAreaEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                lineAreaEntity.Modify(keyValue);
                service.Update(lineAreaEntity);
            }
            else
            {
                lineAreaEntity.Create();
                service.Insert(lineAreaEntity);
            }
        }
    }
}
