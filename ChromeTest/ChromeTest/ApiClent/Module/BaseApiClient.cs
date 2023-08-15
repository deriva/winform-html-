using Newtonsoft.Json;
using System;
using System.Net.Http;
using ChromeTest.Model;

namespace ChromeTest.ApiClient
{

    public class BaseApiClient
    {
        static BaseApiClient()
        {
            Root =    "http://127.0.0.1:10001/";
            Token = "66776";
        }

        #region 封装的底层常用
        public static string Root { get; set; }

        /// <summary>
        /// 登录的时候给它赋值
        /// </summary>
        public static string Token { get; set; }
         

        public static Tuple<int, string> BaseReq(string absurl, string postdata, string type = "Post")
        {
            var flag = 1;
            var result = string.Empty;
            var aur = Root + absurl;
            try
            {
                if (type != "Post") type = "Get";
                var token = Token;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Method", "Post");
                    httpClient.DefaultRequestHeaders.Add("token", token);
                    httpClient.DefaultRequestHeaders.Add("KeepAlive", "false");
                    HttpContent httpContent = new StringContent(postdata, System.Text.Encoding.UTF8, "application/json");

                    if (type == "Post")
                    {
                        var resonse = httpClient.PostAsync(aur, httpContent).Result;
                        result = resonse.Content.ReadAsStringAsync().Result;
                        resonse.Dispose();
                    }
                    else
                    {
                        var resonse = httpClient.GetAsync(aur).Result;
                        result = resonse.Content.ReadAsStringAsync().Result;
                        resonse.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                flag = 0;
                result = aur + "=>" + ex.Message;
            }
            return new Tuple<int, string>(flag, result);
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="absurl"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static ApiResult Featch(string absurl, string postdata = "")
        {
            var r = BaseReq(absurl, postdata, "Post");
            if (r.Item1 == 0) return new ApiResult() { code = 101, message = r.Item2 };
            return JsonConvert.DeserializeObject<ApiResult>(r.Item2);
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="absurl"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static ApiResult FeatchGet(string absurl, string postdata = "")
        {
            var r = BaseReq(absurl, postdata, "Get");
            if (r.Item1==0) return new ApiResult() { code = 101, message = r.Item2 };
            return  JsonConvert.DeserializeObject<ApiResult>(r.Item2);
        }
 

        /// <summary>
        /// 分页用到的：Get
        /// </summary>
        /// <param name="absurl"></param>
        /// <param name="postdata"></param>
        /// <returns></returns> 
        public static ApiPageResult<dynamic> FeatchPageGet(string absurl, string postdata = "")
        {
            var r = BaseReq(absurl, postdata, "Get");
            if (r.Item1 == 0) return new ApiPageResult<dynamic>() { code = 101, message = r.Item2 };
            return JsonConvert.DeserializeObject<ApiPageResult<dynamic>>(r.Item2);
        }


        #endregion
    }


}
