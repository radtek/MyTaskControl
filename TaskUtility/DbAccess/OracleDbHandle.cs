/*===================================================
* 类名称: OracleDbHandle
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/3/29 16:26:15
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace TaskUtility.DbAccess
{
    public abstract class OracleDbHandle
    {

        #region Excute
        /// <summary>  
        /// 执行多条SQL语句，实现数据库事务
        /// </summary>  
        /// <param name="SqlParams">多个SQL参数对象</param>     
        public string ExecuteSqlTran(string ConnectionString, List<OraParameter> SqlParams)
        {
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                //按照名称进行参数替换
                cmd.BindByName = true;
                cmd.Connection = conn;
                OracleTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (var p in SqlParams)
                    {
                        if (p.SqlCmdTxt.Length > 0)
                        {
                            //清空上次循环存在的参数
                            cmd.Parameters.Clear();
                            foreach (var pv in p.Value)
                            {
                                cmd.Parameters.Add(pv);
                            }
                            cmd.CommandText = p.SqlCmdTxt;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return string.Empty;
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException E)
                {
                    tx.Rollback();
                    return E.Message.ToString();
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
        /// 执行SQL语句，返回影响的记录数  
        /// </summary>  
        /// <param name="SQLString">SQL语句</param>  
        /// <returns>影响的记录数</returns>  
        public int ExecuteSql(string ConnectionString, string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.BindByName = true;
                    try
                    {
                        PrepareCommand(cmd, conn, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (Oracle.ManagedDataAccess.Client.OracleException E)
                    {
                        throw new Exception(E.Message);
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

        #region Query
        /// <summary>
        /// 查询DataSet
        /// </summary>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataSet Query(string ConnectionString, OraParameter param)
        {
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand();
                cmd.BindByName = true;
                PrepareCommand(cmd, connection, null, param.SqlCmdTxt, param.Value.ToArray());
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                    return ds;
                }
            }
        }
        private void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        /// <summary>  
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// 返回的是查询到的第一条记录的第一个栏位的值，这个作用主要是用来精确查找的，也就是不会出现select *
        /// 可作为存在值判断
        /// </summary>  
        /// <param name="SQLString">计算查询结果语句</param>  
        /// <returns>查询结果（object）</returns>  
        public object GetSingle(string ConnectionString, string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, conn, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Oracle.ManagedDataAccess.Client.OracleException e)
                    {
                        throw new Exception(e.Message);
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

        #region 执行StoredProcedure
        /// <summary>
        /// 执行无返回值的存储过程（Oracle）
        /// 结果为空时执行成功，不为空时为执行失败的错误提示
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="ProcParam"></param>
        /// <returns></returns>
        public string ExecProcNoReturn(string ConnectionString,OraParameter ProcParam)
        {
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(ProcParam.SqlCmdTxt, conn))
                {
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var p in ProcParam.Value)
                    {
                        cmd.Parameters.Add(p);
                    }

                    OracleTransaction tx = conn.BeginTransaction();
                    cmd.Transaction = tx;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tx.Commit();
                        return string.Empty;
                    }
                    catch (Oracle.ManagedDataAccess.Client.OracleException E)
                    {
                        tx.Rollback();
                        return E.Message.ToString();
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
        /// <summary>
        /// 执行有返回值的存储过程（Oracle）
        /// 方法结果不为空时，结果为错误信息提示；执行成功是方法返回结果为NULL
        /// </summary>
        /// <param name="ProcName">过程名称</param>
        /// <param name="ProcParam">参数</param>
        /// <param name="OutKey">返回值Key</param>
        /// <param name="OutKeyValue">实际返回键值</param>
        /// <returns></returns>
        public string ExecProc(string ConnectionString, string ProcName, OraParameter ProcParam,  ref IDictionary<string, object> OutKeyValue)
        {
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(ProcName, conn))
                {
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var p in ProcParam.Value)
                    {
                        cmd.Parameters.Add(p);
                    }

                    OracleTransaction tx = conn.BeginTransaction();
                    cmd.Transaction = tx;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        tx.Commit();
                        //值返回
                        foreach (var pKey in ProcParam.ProcParametersName)
                        {
                            if (cmd.Parameters.Contains(pKey))
                            {
                                OutKeyValue.Add(pKey, cmd.Parameters[pKey].Value);
                            }
                        }
                        return string.Empty;
                    }
                    catch (Oracle.ManagedDataAccess.Client.OracleException E)
                    {
                        tx.Rollback();
                        return E.Message.ToString();
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
    }
}
