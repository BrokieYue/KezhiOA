using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;

namespace Kezhi.Data
{
    /// <summary>
    /// SQL Server操作类
    /// </summary>
    public class SQLCommon
    {
        /// <summary>
        /// SQL Server数据库的连接
        /// </summary>
        public static SqlConnection SqlConn;
        /// <summary>
        /// SQL Server连接字符串
        /// </summary>
        public static string strConnection =ConfigurationManager.ConnectionStrings["NFineDbContext"].ConnectionString;
        /// <summary>
        /// SQL Server操作类
        /// </summary>
        /// <param name="strConn">SQL Server连接字符串</param>
        public SQLCommon()
        {
            //
            //TODO: 在此处添加构造函数逻辑 
            //
            SqlConn = new SqlConnection(strConnection);
        }
        
        /// <summary>
        /// 克隆SqlParameter（SqlParameter需要循环利用时用到）
        /// </summary>
        /// <param name="para">SqlParameter</param>
        /// <returns>SqlParameter</returns>
        public static SqlParameter CloneParameter(SqlParameter para)
        {
            return (SqlParameter)((ICloneable)para).Clone();
        }

        #region 执行SQL语句
        /// <summary>
        /// 执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string strSql)
        {
            //SqlConnection conn = SqlConn;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(strSql, conn);
                    sda.Fill(ds);
                    return ds;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                //conn.Close();
            }

        }
        /// <summary>
        /// 执行SQL语句，返回DataTable
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string strSql)
        {
            //SqlConnection conn = SqlConn;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(strSql, conn);
                    sda.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                //conn.Close();
            }

        }
        /// <summary>
        /// 执行SQL语句(Insert/Update/Delete)
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteSQL(string strSql)
        {
            //SqlConnection conn = SqlConn;
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    SqlCommand sqlCmd = new SqlCommand(strSql, conn);
                    conn.Open();
                    return sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //conn.Close();
            }
        }
        #endregion


        #region 针对存储过程(Procedure)的操作
        /// <summary>
        /// 执行存储过程，无返回值
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="para">参数</param>
        public void ExecuteNoQuery(string procName, SqlParameter[] para)
        {
            try
            {
                //object[] paraValues = null;
                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    SqlCommand comm = new SqlCommand(procName, conn);
                    if (para != null)
                        comm.Parameters.AddRange(para);
                    comm.CommandType = CommandType.StoredProcedure;
                    // AddInParaValues(comm, procName, paraValues);
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //if (SqlConn.State == ConnectionState.Open)
                //    SqlConn.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 执行存储过程，有返回值
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="para">参数</param>
        /// <returns>返回值Int型(返回值必须为最后一个参数output)</returns>
        public int ExecuteReturnID(string procName, SqlParameter[] para)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    SqlCommand comm = new SqlCommand(procName, conn);
                    if (para != null)
                        comm.Parameters.AddRange(para);
                    comm.Parameters.Add("ReturnID", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    comm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    int iRow = Convert.ToInt32(comm.Parameters[para.Length - 1].Value); //注意存储过程中要将记录返回值ID的变量放到最后
                    // int iRow =Convert.ToInt32(comm.Parameters["SelectedID"].Value);// comm.ExecuteNonQuery();
                    conn.Close();
                    return iRow;
                }
            }
            catch (Exception ex)
            {
                //if (SqlConn.State == ConnectionState.Open)
                //    SqlConn.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 执行存储过程，返回一个DataTable
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="para">参数SqlParameter[]</param>
        /// <returns>数据集DataTable</returns>
        public DataTable ExecuteDataTable(string procName, SqlParameter[] para)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    object[] paraValues = null;
                    SqlCommand comm = new SqlCommand(procName, conn);
                    if (para != null)
                        comm.Parameters.AddRange(para);
                    comm.CommandType = CommandType.StoredProcedure;
                    AddInParaValues(comm, procName, paraValues);

                    SqlDataAdapter sda = new SqlDataAdapter(comm);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                //if (SqlConn.State == ConnectionState.Open)
                //    SqlConn.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 执行存储过程，返回SqlDataReader对象
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="paraValues">要传递给给存储过程的参数值类表</param>
        /// <returns>返回SqlDataReader对象</returns>
        public SqlDataReader ExecuteDataReader(string procName, params object[] paraValues)
        {
            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                SqlCommand comm = new SqlCommand(procName, conn);
                comm.CommandType = CommandType.StoredProcedure;
                AddInParaValues(comm, procName, paraValues);
                conn.Open();
                return comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        // 为 SqlCommand 添加参数及赋值。   
        private void AddInParaValues(SqlCommand comm, string procName, params object[] paraValues)
        {
            comm.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int));
            comm.Parameters["@RETURN_VALUE"].Direction =
                            ParameterDirection.ReturnValue;
            if (paraValues != null)
            {
                ArrayList al = GetParas(procName);
                for (int i = 0; i < paraValues.Length; i++)
                {
                    comm.Parameters.AddWithValue(al[i + 1].ToString(),
                       paraValues[i]);
                }
            }
        }

        /// <summary>
        /// 获取存储过程的参数列表
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public ArrayList GetParas(string procName)
        {
            SqlCommand comm = new SqlCommand("dbo.sp_sproc_columns_90", SqlConn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddWithValue("@procedure_name", (object)procName);
            SqlDataAdapter sda = new SqlDataAdapter(comm);

            DataTable dt = new DataTable();
            sda.Fill(dt);

            ArrayList al = new ArrayList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                al.Add(dt.Rows[i][3].ToString());
            }
            return al;
        }

        #endregion

        /// <summary>
        /// 执行事务处理SQL
        /// </summary>
        public  bool ExecTSQL(string[] sqls)
        {
            using (SqlConnection con = new SqlConnection(strConnection))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    for (int i = 0; i < sqls.Length; i++)
                    {
                        if (sqls[i] == "" || sqls[i] == null)
                        {
                            continue;
                        }
                        SqlCommand cmd = con.CreateCommand();
                        cmd.Transaction = trans;
                        cmd.CommandText = sqls[i];
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback(); //2018-7-11
                    con.Close();
                    return false;
                }
                finally
                {
                    trans = null;
                    con.Close();
                }
            }
        }

        public  bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public  object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

    }
}
