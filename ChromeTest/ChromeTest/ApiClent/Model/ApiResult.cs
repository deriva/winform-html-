using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ChromeTest.Model
{
    /// <summary>
    /// 统一接口返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ApiResult
    {

        /// <summary>
        /// 请求状态
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }


        /// <summary>
        /// 自定义属性
        /// </summary>
        public object attr { get; set; }

        /// <summary>
        /// 自定义属性
        /// </summary>
        public object Data { get; set; }
    }
    
    /// <summary>
    /// 返回带分页的Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiPageResult<T> : ApiResult
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get { return Page > 0; }
        }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return Page + 1 < TotalPages; }
        }

        public List<T> Items { get; set; }

        public object TotalField { get; set; }
    }

    //将JObject转换为DymamicObject
    public class JObjectAccessor : DynamicObject
    {
        private JToken obj;

        public JObjectAccessor(JToken obj)
        {
            this.obj = obj;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var val = obj?[binder.Name];
            if (val == null) return false;
            result = Populate(val);
            return true;
        }

        private object Populate(JToken token)
        {
            var val = token as JValue;
            if (val != null) return val.Value;
            else if (token.Type == JTokenType.Array)
            {
                var list = new List<object>();
                foreach (var item in token as JArray)
                {
                    list.Add(Populate(item));
                }

                return list;
            }
            else return new JObjectAccessor(token);
        }
    }

    public enum StatusCodeType
    {
        /// <summary>
        /// 请求(或处理)成功  请用code=100
        /// </summary>
        Success = 200,

        ///// <summary>
        ///// 内部请求出错
        ///// </summary>
        //[Text("内部请求出错")]
        //InnerError = 500,

        ///// <summary>
        ///// 访问请求未授权! 当前 SESSION 失效, 请重新登陆
        ///// </summary>
        //[Text("访问请求未授权! 当前 SESSION 失效, 请重新登陆")]
        //Unauthorized = 401,

        /// <summary>
        /// 请求参数不完整或不正确
        /// </summary> 
        ParameterError = 400,

        ///// <summary>
        ///// 您无权进行此操作，请求执行已拒绝
        ///// </summary>
        //[Text("您无权进行此操作，请求执行已拒绝")]
        //Forbidden = 403,

        ///// <summary>
        ///// 找不到与请求匹配的 HTTP 资源
        ///// </summary>
        //[Text("找不到与请求匹配的 HTTP 资源")]
        //NotFound = 404,

        ///// <summary>
        ///// HTTP请求类型不合法
        ///// </summary>
        //[Text("HTTP请求类型不合法")]
        //HttpMehtodError = 405,

        ///// <summary>
        ///// HTTP请求不合法,请求参数可能被篡改
        ///// </summary>
        //[Text("HTTP请求不合法,请求参数可能被篡改")]
        //HttpRequestError = 406,

        ///// <summary>
        ///// 该URL已经失效
        ///// </summary>
        //[Text("该URL已经失效")]
        //URLExpireError = 407,

        ///// <summary>
        ///// 已过期
        ///// </summary>
        //[Text("已过期")]
        //Expire = 408,

        /// <summary>
        /// 请求成功
        /// </summary>
        ApiSuccess = 100,

        /// <summary>
        /// 请求失败
        /// </summary>
        Fail = 101,

        /// <summary>
        /// 请求错误
        /// </summary>
        Error = -1,

        /// <summary>
        /// 未登录
        /// </summary>
        NotLogin = 102,

        /// <summary>
        /// 时间戳已过期
        /// </summary>
        TimeStampExipre = 103,

        /// <summary>
        /// 访问评率太高
        /// </summary>
        FrequencyHigh = 104,

        /// <summary>
        /// Token失效
        /// </summary>
        TokenLost = 105,

        /// <summary>
        /// 程序错误
        /// </summary>
        ProgramError = 106,

        /// <summary>
        /// 未授权
        /// </summary>
        PowerLost = 107,
    }
}
