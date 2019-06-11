using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskUtility.Common;

namespace TaskUtility.Interface
{
    /// <summary>
    /// 上传功能接口
    /// </summary>
    public interface IUpTask
    {
        /// <summary>
        /// 上传处理
        /// </summary>
        bool UpHandle(UpTaskParamter upParam,out string errInfo);
    }
}
