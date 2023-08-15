using CefSharp.WinForms;
using ChromeTest.ApiClient;
using ChromeTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ChromeTest
{
    /// <summary>
    /// Chrome 工具
    /// </summary>
    public class ChromeUtils
    {

        public static ChromiumWebBrowser m_chromeBrowser = null;
        // P/Invoke constants
        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;

        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        // ID for the Chrome dev tools item on the system menu
        private int SYSMENU_CHROME_DEV_TOOLS = 0x1;
    }


    /// <summary>
    /// 基础的js对象
    /// </summary>
    public class BaseJsObj
    {

        /// <summary>
        /// 页面地址参数
        /// </summary>
        public string UrlParm { get; set; }

        /// <summary>
        /// 窗体名称
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// 反射方法
        /// </summary>
        /// <param name="path">{命名空间}/{类名}/{方法名}</param>
        /// <param name="parm"></param>
        public object Invoke(string path, Dictionary<string, object> parm)
        {
            try
            {
                // var tt=  JObject.Parse(parm);
                var namespaceStr = path.Split('/')[0];
                var classStr = path.Split('/')[1];
                var funcationStr = path.Split('/')[2];
                Assembly assembly = Assembly.LoadFrom("WareHouseNew.dll"); //获取包含当前代码的程序集 
                object o = assembly.CreateInstance(namespaceStr + "." + classStr); //这里所述的完整类名指的是包括名称空间，即：名称空间.类名

                var tt = new object[] { parm };
                ///获取方法
                MethodInfo method_info = o.GetType().GetMethod(funcationStr,
                                       BindingFlags.Public | BindingFlags.Instance);
                //执行方法 object[]数组内为参数
                //if (parm == null || parm == "") { return method_info?.Invoke(o, new object[] { }); }
                return method_info?.Invoke(o, tt);
            }
            catch (Exception ex)
            {
                return new ApiResult() { code = 101, message = ex.Message };
            }
        }
        /// <summary>
        /// get请求方法
        /// </summary>
        /// <param name="path">路径：api/xxx/asas?id=12</param> 
        public ApiResult FeatchGet(string path)
        {
            try
            {
                return BaseApiClient.FeatchGet(path);
            }
            catch (Exception ex)
            {
                return new ApiResult() { code = 101, message = ex.Message };
            }
        }
        /// <summary>
        /// get分页请求方法
        /// </summary>
        /// <param name="path">路径：api/xxx/asas?id=12</param> 
        public string FeatchPageGet(string path)
        {
            try
            {
                var r = BaseApiClient.BaseReq(path, "", "Get");
                if (r.Item1 == 0) return JsonConvert.SerializeObject(new ApiPageResult<dynamic>() { code = 101, message = r.Item2 });
                return r.Item2;

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ApiPageResult<dynamic> { code = 101, message = ex.Message });
            }
        }
        /// <summary>
        /// post请求方法
        /// </summary>
        /// <param name="path">路径：api/xxx/asas?id=12</param> 
        public ApiResult Featch(string path, Dictionary<string, object> parm)
        {
            try
            {
                var data = "";
                if (parm != null) data = Newtonsoft.Json.JsonConvert.SerializeObject(parm);
                return BaseApiClient.Featch(path, data);
            }
            catch (Exception ex)
            {
                return new ApiResult() { code = 101, message = ex.Message };
            }
        }

        /// <summary>
        /// 获取Url的参数
        /// </summary> 
        public object GetUrlParm()
        {
            return UrlParm;
        }

        /// <summary>
        ///关闭窗体
        /// </summary> 
        public void CloseForm()
        {
            if (!string.IsNullOrWhiteSpace(FormName))
            {
                Form fm2 = null;
                FormCollection formCollection = Application.OpenForms;
                foreach (Form fm in formCollection)
                {
                    if (fm.Name == FormName)     //使用委托
                    {
                        fm2 = fm;                //创建委托、绑定委托、调用委托
                        break;
                    }
                }
                if (fm2 != null)
                {
                    Thread thread1 = new Thread(() =>
                    {
                        if (fm2.InvokeRequired)
                            fm2.Invoke(new Action(() => { fm2.Close(); }));
                        else fm2.Close();
                    });
                    thread1.Start();

                }
            }
        }

        /// <summary>
        /// 指定的窗体刷新
        /// </summary>
        public void FormParentRefresh(Dictionary<string, object> parm = null)
        {
            if (!string.IsNullOrWhiteSpace(FormName))
            {
                Form fm2 = null;
                FormCollection formCollection = Application.OpenForms;
                foreach (Form fm in formCollection)
                {
                    if (fm.Name == FormName)     //使用委托
                    {
                        fm2 = fm;                //创建委托、绑定委托、调用委托
                        break;
                    }
                }
                if (fm2 != null)
                {
                    Thread thread1 = new Thread(() =>
                    {
                        var f3 = fm2 as frmChrome;
                        f3.FormCallBackParent(parm);
                    });
                    thread1.Start();

                }
            }
        }

    }

}
