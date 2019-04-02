/*===================================================
* 类名称: SqlDbHandle
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/4/2 10:14:19
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace TaskUtility.DbAccess
{
    public class SqlDbHandle
    {
        #region Execute
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="ConnString"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public int ExcuteSQL(string ConnString,SqlServerParameter paramValue)
        {
            int i = 0;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlCommand cmd = new SqlCommand(paramValue.SqlCmdTxt, conn);
                cmd.CommandType = CommandType.Text;
                foreach (var item in paramValue.Value)
                {
                    cmd.Parameters.Add(item);
                }
                conn.Open();
                i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return i;
        }
        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="ConnString"></param>
        /// <param name="paramValues"></param>
        public string ExcuteSQLs(string ConnString, List<SqlServerParameter> paramValues)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    SqlTransaction tran = conn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        foreach (var item in paramValues)
                        {
                            if (item.SqlCmdTxt.Length > 0)
                            {
                                //清空上次循环存在的参数
                                cmd.Parameters.Clear();
                                foreach (var v in item.Value)
                                {
                                    cmd.Parameters.Add(v);
                                }
                            }
                            cmd.CommandText = item.SqlCmdTxt;
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                        return string.Empty;

                    }
                    catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                    {
                        tran.Rollback();
                        return ex.ToString();

                    }
                    finally
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
        #endregion

        #region 执行 StoredProcedure
        /// <summary>
        /// 执行无返回值存储过程（SQLServer）
        /// </summary>
        /// <param name="ConnString"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string ExecProcNoReturn(string ConnString,SqlServerParameter parameter)
        {
            using (SqlConnection conn  = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = parameter.SqlCmdTxt;
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    foreach (var item in parameter.Value)
                    {
                        cmd.Parameters.Add(item);
                    }
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return ex.ToString();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }

        }
        /// <summary>
        /// 执行有返回值存储过程（SQLServer）
        /// </summary>
        /// <param name="ConnString">连接串</param>
        /// <param name="parameter"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public string ExecProc(string ConnString, SqlServerParameter parameter, ref IDictionary<string, object> Result)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = parameter.SqlCmdTxt
                };
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    foreach (var item in parameter.Value)
                    {
                        cmd.Parameters.Add(item);
                    }
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    foreach (var pName in parameter.ProcParameterName)
                    {
                        if (cmd.Parameters.Contains(pName))
                        {
                            Result.Add(pName, cmd.Parameters[pName].Value);
                        }
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Result = null;
                    return ex.ToString();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region Query
        public DataSet Query(string ConnString,SqlServerParameter parameter)
        {
            DataSet dt = new DataSet(); ;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter da = new SqlDataAdapter(parameter.SqlCmdTxt, conn);
                da.SelectCommand.CommandType =  CommandType.Text;
                foreach (var item in parameter.Value)
                {
                    da.SelectCommand.Parameters.Add(item);
                }
                da.Fill(dt);
                if (dt != null && dt.Tables.Count == 2)
                {
                    dt.Tables[0].TableName = "DataList";
                    dt.Tables[1].TableName = "DataTotal";
                }
                else if (dt != null && dt.Tables.Count == 1)
                {
                    dt.Tables[0].TableName = "DataList";
                }
            }
            return dt;
        }
        #endregion
        //----
    }
}
