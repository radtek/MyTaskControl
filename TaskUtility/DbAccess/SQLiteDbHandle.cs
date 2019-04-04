/*===================================================
* 类名称: SQLiteDbHandle
* 类描述: SQLite数据操作基类
* 创建人: 李先锋
* 创建时间: 2019/4/4 14:04:38
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace TaskUtility.DbAccess
{
    public class SQLiteDbHandle
    {
        /// <summary>
        /// 增、删、改（SQLite）
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int SQLiteExecute(string dbPath,SQLIteDbParameter param)
        {
            using (SQLiteConnection conn = new SQLiteConnection($"data source={dbPath}"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = param.SqlCmdTxt;
                    foreach (var item in param.SqlLiteParameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                    return cmd.ExecuteNonQuery();
                }

            }
        }

        public DataSet SQLiteGetDataSet(string dbPath,SQLIteDbParameter param)
        {
            using (SQLiteConnection conn = new SQLiteConnection($"data source={dbPath}"))
            {
                conn.Open();
                DataSet dt = new DataSet();
                using (SQLiteDataAdapter da = new SQLiteDataAdapter())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = param.SqlCmdTxt;
                        foreach (var item in param.SqlLiteParameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                        da.SelectCommand = cmd;
                    }
                    da.Fill(dt);
                    return dt;
                }

            }

        }
    }
}
