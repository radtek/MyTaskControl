/*===================================================
* 类名称: DownTaskParamter
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/3/29 16:44:36
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtility.Common
{
    public class DownTaskParamter
    {
        /// <summary>
        /// 下载任务编号
        /// </summary>
        public string DownTaskCode { get; set; }
        /// <summary>
        /// 下载任务子编号
        /// </summary>
        public string DownTaskSubCode { get; set; }
        /// <summary>
        /// 下载任务描述
        /// </summary>
        public string DownTaskDesc { get; set; }
        /// <summary>
        /// 下载任务SQL语句
        /// </summary>
        public string SqlTxt { get; set; }
        /// <summary>
        /// 查询结果DataTable
        /// </summary>
        public DataTable SoureTable { get; set; }
        /// <summary>
        /// 查询结果DataSet
        /// </summary>
        public DataSet SourceSet { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public enum DataBaseType
        {
            SQLServer = 0,
            Oracle = 1,
            ManageDb = 2
        }
    }
}
