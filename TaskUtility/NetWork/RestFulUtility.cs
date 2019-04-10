using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace TaskUtility.NetWork
{
    /// <summary>
    /// RestFul规范接口通用类
    /// </summary>
    public class RestFulUtility
    {
        public string aa()
        {
            RestClient client = new RestClient("http://baidu.com");
            RestRequest request = new RestRequest("resource/{id}", Method.POST);
            //QueryString参数添加
            request.AddParameter("name", "value");
            //请求头参数添加
            request.AddHeader("header", "value");
            //请求文件
            request.AddFile("FileName","FilePath");
            var postdata = new
            {
                username = "yanyangtian",
                password = "123456",
                nickname = "艳阳天"
            };
            var json = request.JsonSerializer.Serialize(postdata);
            request.AddParameter("application/json; charset=utf-8",json, ParameterType.RequestBody);
            // execute the request
            IRestResponse response = client.Execute(request);
            IRestResponse<Person> response2 = client.Execute<Person>(request);
            
            var name = response2.Data.Name;
            return name;
        }
    }
}
