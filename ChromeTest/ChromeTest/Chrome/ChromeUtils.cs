using CefSharp.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
 

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
                Assembly assembly = Assembly.LoadFrom("ToothFacManage.dll"); //获取包含当前代码的程序集 
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
                return ResultHelper.ToFail(ex.Message);
            }
        }
    }



}
