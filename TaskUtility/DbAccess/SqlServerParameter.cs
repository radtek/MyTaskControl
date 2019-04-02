/*===================================================
* 类名称: SqlServerParameter
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/3/30 13:14:05
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
using System.Data.SqlClient;


namespace TaskUtility.DbAccess
{
    /// <summary>
    /// SqlServerDb参数
    /// </summary>
    public class SqlServerParameter
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        public string SqlCmdTxt { get; set; }
        /// <summary>
        /// Oracle参数值
        /// </summary>
        public List<SqlParameter> Value { get; set; }
        /// <summary>
        /// 过程参数名称（返回值）
        /// </summary>
        public string[] ProcParameterName { get; set; }

    }
}
