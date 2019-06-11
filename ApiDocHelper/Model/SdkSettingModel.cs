using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDocHelper.Model
{
    public class SdkSettingModel
    {
        /// <summary>
        /// 方法中文释义
        /// </summary>
        public string FuncChineseName { get; set; }
        /// <summary>
        /// 方法位置（方法命名空间）
        /// </summary>
        public string FuncPosition { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string FuncName { get; set; }
        /// <summary>
        /// 返回结果类型
        /// </summary>
        public string ReturnType { get; set; }
        /// <summary>
        /// 方法备注
        /// </summary>
        public string FuncDesc { get; set; }
        /// <summary>
        /// 方法入参说明
        /// </summary>
        public List<ParamsSetting> InputParams { get; set; }
        /// <summary>
        /// 方法出参说明
        /// </summary>
        public List<ParamsSetting> OutParams { get; set; }
        /// <summary>
        /// 返回结果备注
        /// </summary>
        public string ReturnParamsDesc { get; set; }
    }
}
