using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDocHelper.Model
{
    public class ParamsSetting
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 类型长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 能否为空
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// 参数说明
        /// </summary>
        public string Desc { get; set; }

    }
}
