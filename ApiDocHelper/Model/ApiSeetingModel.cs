using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDocHelper.Model
{
    public class ApiSeetingModel
    {
        /// <summary>
        /// Api名称
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        /// Api更新时间
        /// </summary>
        public string ApiUpdateTime { get; set; } = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        /// <summary>
        /// 功能介绍
        /// </summary>
        public string ApiDesc { get; set; }
        /// <summary>
        /// Url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求方式：Get/Post
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 编码规范
        /// </summary>
        public string Encoding { get; set; }
        /// <summary>
        /// 公共请求消息头/消息头（Header）
        /// </summary>
        public string RequestHeader { get; set; }
        /// <summary>
        /// 公共响应消息头/公共响应头域
        /// </summary>
        public string ResponseHeader { get; set; }
        /// <summary>
        /// 额外的请求头
        /// </summary>
        public List<ParamsSetting> RequestExtraHeader{ get; set; }
        /// <summary>
        /// 请求体参数说明
        /// </summary>
        public List<ParamsSetting> RequestBodyJson { get; set; }
        /// <summary>
        /// 响应参数说明
        /// </summary>
        public List<ParamsSetting> ResponseResult { get; set; }
    }
}
