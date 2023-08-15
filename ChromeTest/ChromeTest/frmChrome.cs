using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChromeTest
{
    public partial class frmChrome : Form
    {
        private string _page;
        private string _formName;
        private ChromiumWebBrowser m_chromeBrowser = null;
        /// <summary>
        /// 父页面事件
        /// </summary>
        public event Action<Dictionary<string, object>> EventParent;
        public frmChrome()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="page">页面地址:html/page下的相对路径</param>
        /// <param name="title">表单标题</param>
        public frmChrome(string page, string title, string formName = "")
        {
            InitializeComponent();
            _page = page;
            if (!string.IsNullOrWhiteSpace(title))
                this.Text = title;
            if (!string.IsNullOrWhiteSpace(formName))
                this._formName = formName;
        }
        private void frmChrome_Load(object sender, EventArgs e)
        {
            try
            {
                InitCefSettings();


                if (string.IsNullOrWhiteSpace(_page))
                {
                    _page = "err.html";
                }
                var p1 = _page; var p2 = string.Empty;
                if (_page.Contains("?"))
                {
                    p1 = _page.Split('?')[0];
                    p2 = _page.Split('?')[1];
                }
                var page = new Uri(string.Format("file:///{0}html/page/{1}", AppDomain.CurrentDomain.BaseDirectory, p1));
                m_chromeBrowser = new ChromiumWebBrowser(page.ToString());

                m_chromeBrowser.FrameLoadEnd += OnFrameLoadEnd;
                m_chromeBrowser.KeyboardHandler = new CEFKeyBoardHander();
                panel1.Controls.Add(m_chromeBrowser);
                ChromeDevToolsSystemMenu.CreateSysMenu(this);
                var baseJsObj = new BaseJsObj();
                baseJsObj.UrlParm = p2;
                baseJsObj.FormName = _formName;
                m_chromeBrowser.RegisterJsObject("winformObj", baseJsObj, false);



            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("CefSharp.Core.dll") && ex.Message.Contains("加载"))
                {
                    MessageBox.Show("运行此页面需要下载:Visual C++ Redistributable Packages for Visual Studio 2013. 点击下载.下载安装后，重新运行【义齿软件】");
                    System.Diagnostics.Process.Start("https://download.microsoft.com/download/F/3/5/F3500770-8A08-488E-94B6-17A1E1DD526F/vcredist_x86.exe");
                }
                else
                    MessageBox.Show(ex.Message);
            }

        }
        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {


        }

        private void InitCefSettings()
        {
            try
            {
                if (!Cef.IsInitialized)
                {
                    //有些电脑的显卡处理有问题  所以此处关闭掉
                    var settings = new CefSettings
                    {
                        Locale = "zh-CN",
                        AcceptLanguageList = "zh-CN",
                        MultiThreadedMessageLoop = true,
                    };
                    settings.CefCommandLineArgs.Add("disable-gpu", "1"); // 禁用gpu
                    Cef.Initialize(settings);
                }
            }
            catch { }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == ChromeDevToolsSystemMenu.WM_SYSCOMMAND) && ((int)m.WParam == ChromeDevToolsSystemMenu.SYSMENU_CHROME_DEV_TOOLS))
            {
                m_chromeBrowser.ShowDevTools();
            }
        }

        private void frmChrome_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Comment out Cef.Shutdown() call - it will be automatically called when exiting the application.
            //Due to a timing issue and the way the WCF service closes it's self in newer versions, it can be best to leave CefSharp to clean it's self up.
            //Alternative solution is to set the WCF timeout to Zero (or a smaller number) using CefSharp.CefSharpSettings.WcfTimeout = TimeSpan.Zero;
            // This must be done before creating any ChromiumWebBrowser instance
            // Cef.Shutdown();
            //  m_chromeBrowser.Dispose();
        }
        /// <summary>
        /// 页面回调事件
        /// </summary>
        public void FormCallBackParent(Dictionary<string, object> parm = null)
        {
            EventParent(parm);
        }

    }
    public class CEFKeyBoardHander : IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.KeyUp && Enum.IsDefined(typeof(Keys), windowsKeyCode))
            {
                var key = (Keys)windowsKeyCode;
                switch (key)
                {
                    case Keys.F12:
                        browser.ShowDevTools();
                        break;

                    case Keys.F5:

                        if (modifiers == CefEventFlags.ControlDown)
                        {
                            //MessageBox.Show("ctrl+f5");
                            browser.Reload(true); //强制忽略缓存

                        }
                        else
                        {
                            //MessageBox.Show("f5");
                            browser.Reload();
                        }
                        break;

                }
            }
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }
    }
}
