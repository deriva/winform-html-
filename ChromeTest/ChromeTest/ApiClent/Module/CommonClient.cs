using Newtonsoft.Json.Linq;
using System.Collections.Generic;
 
namespace ChromeTest.ApiClient.Module
{
    /// <summary>
    /// 通用的查数据的
    /// </summary>
    public class CommonClient : BaseApiClient
    {
        public static CommonClient Instance
        {
            get
            {
                return new CommonClient();
            }
        }
 
    }
}
