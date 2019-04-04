/*===================================================
* 类名称: SQLIteDbParameter
* 类描述: SQLite参数对象类
* 创建人: 李先锋
* 创建时间: 2019/4/4 14:04:59
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtility.DbAccess
{
    /// <summary>
    /// SQLite参数
    /// </summary>
    public class SQLIteDbParameter
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        public string SqlCmdTxt { get; set; }
        /// <summary>
        /// SQLite参数
        /// </summary>
        public List<SQLiteParameter> SqlLiteParameters { get; set; }
    }
}
