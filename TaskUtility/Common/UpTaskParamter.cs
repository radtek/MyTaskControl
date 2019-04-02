/*===================================================
* 类名称: UpTaskParamter
* 类描述: 上传任务参数对象
* 创建人: 李先锋
* 创建时间: 2019/3/29 16:44:04
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

namespace TaskUtility.Common
{
    /// <summary>
    /// 上传任务参数对象
    /// </summary>
    public class UpTaskParamter
    {
        /// <summary>
        /// 上传任务主编号
        /// </summary>
        public string UpTaskCode { get; set; }
        /// <summary>
        /// 上传任务子编号
        /// </summary>
        public string UpTaskSubCode { get; set; }
        /// <summary>
        /// 上传任务简介
        /// </summary>
        public string UpTaskDesc { get; set; }
        /// <summary>
        /// 数据源SQL语句
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
