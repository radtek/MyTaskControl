/*===================================================
* 类名称: MyTask
* 类描述:
* 创建人: 李先锋
* 创建时间: 2019/3/29 16:25:22
* 修改人: 
* 修改时间:
* 版本： @version 1.0
=====================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskUtility.Interface;
using TaskUtility.DbAccess;
using TaskUtility.Common;

namespace TaskUtility
{
    class MyTask : OracleDbHandle,IUpTask
    {
        public bool UpHandle(UpTaskParamter uptaskParam, out string errInfo)
        {
            
            throw new NotImplementedException();
        }
    }
}
