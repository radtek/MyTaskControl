using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskUtility.Common;

namespace TaskUtility.Interface
{
    /// <summary>
    /// 下载功能接口
    /// </summary>
    public interface IDownTask
    {
        bool DownExcuete(DownTaskParamter downTaskParamster,out string errInfo);
    }
}
