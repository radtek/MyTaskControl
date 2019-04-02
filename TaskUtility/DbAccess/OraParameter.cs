/*===================================================
* 类名称: OraParameter
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/3/30 10:35:46
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace TaskUtility.DbAccess
{
    /// <summary>
    /// OracleDb参数
    /// </summary>
    public class OraParameter
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        public string SqlCmdTxt { get; set; }
        /// <summary>
        /// Oracle参数值
        /// </summary>
        public List<OracleParameter> Value { get; set; }
        /// <summary>
        /// 存储过程返回值名称
        /// </summary>
        public string[] ProcParametersName { get; set; }
    }
}
