﻿using CefSharp;
using CefSharp.WinForms;
using ChromeTest.Ball;
using ChromeTest.Demos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeTest
{
    public partial class MainForm : Form
    {



        public static string GetAppLocation()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenPage();
            //var bj = "3,6,4,0,2,3,1,2,0,8,9,3,3,0,0,0,4,6,2,8,7,2,5,9,4,7,0,6,8,5,5,9,8,0,3,0,1,5,1,6,5,1,2,2,4,5,3,1,2,8,4,0,6,1,3,3,7,4,2,1,7,3,7,3,1,7,0,2,9,9,5,9,7,8,7,7,3,2,4,3,9,3,4,8,9,3,9,4,0,7,4,6,5,2,2,0,7,0,0,8,5,5,6,5,9,3,1,5,6,2,4,8,5,3,0,5,7,7,2,6,6,8,3,5,6,0,1,8,8,0,4,4,8,5,3,0,9,7,3,1,5,8,1,3,6,8,9,4,1,8,1,1,1,3,7,6,0,1,3,1,8,1,9,7,0,2,7,0,2,5,1,0,7,0,1,4,5,1,9,8,4,8,8,8,9,0,8,8,5,1,2,7,1,2,7,8,5,0,5,8,1,1,3,6,4,4,3,8,6,0,2,0,6,0,5,1,7,6,4,7,1,8,2,4,8,2,0,4,7,5,1,1,2,7,1,0,7,6,4,8,5,9,1,9,6,0,4,7,5,2,4,3,6,6,0,8,8,2,5,0,2,1,2,2,9,3,5,1,5,8,2,1,7,6,9,8,1,3,3,3,6,0,6,3,6,4,1,8,2,0";

            //AlgorithmHelper.CalcLisahn2(bj);

            //var tt = bj.Split(',').ToList();
            //tt= tt.Skip(tt.Count - 250).ToList();
            //AlgorithmHelper.FanTuiNext(0, string.Join(",", tt));
        }
        private void OpenWebSite()
        {
            ChromeUtils.m_chromeBrowser = new ChromiumWebBrowser("http://www.maps.google.com");

            panel1.Controls.Add(ChromeUtils.m_chromeBrowser);

            ChromeDevToolsSystemMenu.CreateSysMenu(this);


        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == ChromeDevToolsSystemMenu.WM_SYSCOMMAND) && ((int)m.WParam == ChromeDevToolsSystemMenu.SYSMENU_CHROME_DEV_TOOLS))
            {
                ChromeUtils.m_chromeBrowser.ShowDevTools();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Comment out Cef.Shutdown() call - it will be automatically called when exiting the application.
            //Due to a timing issue and the way the WCF service closes it's self in newer versions, it can be best to leave CefSharp to clean it's self up.
            //Alternative solution is to set the WCF timeout to Zero (or a smaller number) using CefSharp.CefSharpSettings.WcfTimeout = TimeSpan.Zero;
            // This must be done before creating any ChromiumWebBrowser instance
            //Cef.Shutdown();
        }

        private void buttonShowDevTools_Click(object sender, EventArgs e)
        {
            ChromeUtils.m_chromeBrowser.ShowDevTools();
        }

        private void buttonChangeAddress_Click(object sender, EventArgs e)
        {
            ChangeAddressForm form = new ChangeAddressForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ChromeUtils.m_chromeBrowser.Load(form.textBoxAddress.Text);
            }
        }

        private void buttonCustomHTML_Click(object sender, EventArgs e)
        {
            ChromeUtils.m_chromeBrowser.LoadHtml("<html><body>Hello world</body></html>", "http://customrendering/");
        }

        private void buttonLoadLocalHTML_Click(object sender, EventArgs e)
        {
            //string page = string.Format("{0}HTMLResources/html/BasicPage.html", GetAppLocation());
            //string page = string.Format("{0}HTMLResources/html/BootstrapExample.html", GetAppLocation());

            var page = new Uri(string.Format("file:///{0}HTMLResources/html/BootstrapExample.html", GetAppLocation()));

            ChromeUtils.m_chromeBrowser.Load(page.ToString());
        }



        private void buttonLoadEmbeddedHTML_Click(object sender, EventArgs e)
        {
            //string resource = "";
            //if (EmbeddedResourceUtils.GetResource("BootstrapExample.html", out resource) == true)
            //{
            //    m_chromeBrowser.LoadHtml(resource, "http://customrendering/");
            //}
        }

        JavaScriptInteractionObj m_jsInteractionObj = null;

        private void buttonRegisterCSharpObject_Click(object sender, EventArgs e)
        {
            //To register a JS object, it must occur immediate after the browser has been created
            //So in this instance we'll create a new browser;
            panel1.Controls.Remove(ChromeUtils.m_chromeBrowser);

            var page = new Uri(string.Format("file:///{0}HTMLResources/html/WinformInteractionExample.html", GetAppLocation()));

            ChromeUtils.m_chromeBrowser = new ChromiumWebBrowser(page.ToString());

            panel1.Controls.Add(ChromeUtils.m_chromeBrowser);

            m_jsInteractionObj = new JavaScriptInteractionObj();

            // Register the JavaScriptInteractionObj class with JS
            ChromeUtils.m_chromeBrowser.RegisterJsObject("winformObj", m_jsInteractionObj);
        }

        private void buttonExecJavaScriptFromWinforms_Click(object sender, EventArgs e)
        {
            ChromeUtils.m_chromeBrowser.LoadHtml("<html><body>Hello world</body></html>", "http://customrendering/");

            var script = "document.body.style.backgroundColor = 'red';";

            ChromeUtils.m_chromeBrowser.ExecuteScriptAsync(script);
        }

        private void buttonAmCharts_Click(object sender, EventArgs e)
        {
            // m_chromeBrowser.Load("http://www.amcharts.com/demos/date-based-data/");

            string page = string.Format("{0}HTMLResources/html/amChartExample.html", GetAppLocation());
            ChromeUtils.m_chromeBrowser.Load(page);
        }

        private void buttonBootStrapFormDemo_Click(object sender, EventArgs e)
        {
            BootStrapForm form = new BootStrapForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("user pressed ok");
            }
        }

        private void buttonHTMLDemos_Click(object sender, EventArgs e)
        {
            DemoLauncherForm form = new DemoLauncherForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("user pressed ok");
            }
        }

        private void buttonExecCsharpFromJS_Click(object sender, EventArgs e)
        {

        }

        private void buttonReturnDataFromJavaScript_Click(object sender, EventArgs e)
        {
            ChromeUtils.m_chromeBrowser.LoadHtml("<html><body>Hello world</body></html>", "http://customrendering/");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function tempFunction() {");
            sb.AppendLine("     var w = window.innerWidth;");
            sb.AppendLine("     var h = window.innerHeight;");
            sb.AppendLine("");
            sb.AppendLine("     return w*h;");
            sb.AppendLine("}");
            sb.AppendLine("tempFunction();");

            var task = ChromeUtils.m_chromeBrowser.EvaluateScriptAsync(sb.ToString());

            task.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;

                    if (response.Success == true)
                    {
                        MessageBox.Show(response.Result.ToString());
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void buttonReturnDataFromJavaScript2_Click(object sender, EventArgs e)
        {
            // Step 01: create a simple html page (include jquery so we have access to json object
            StringBuilder htmlPage = new StringBuilder();
            htmlPage.AppendLine("<html>");
            htmlPage.AppendLine("<head>");
            htmlPage.AppendLine("<script type=\"text/javascript\" src=\"http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js\"> </script>");
            htmlPage.AppendLine("</head>");
            htmlPage.AppendLine("<body>Hello world 2</body>");
            htmlPage.AppendLine("</html>");

            // Step 02: Load the Page
            ChromeUtils.m_chromeBrowser.LoadHtml(htmlPage.ToString(), "http://customrendering/");

            // Step 03: Execute some ad-hoc JS that returns an object back to C#
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function tempFunction() {");
            sb.AppendLine("     // create a JS object");
            sb.AppendLine("     var person = {firstName:'John', lastName:'Maclaine', age:23, eyeColor:'blue'};");
            sb.AppendLine("");
            sb.AppendLine("     // Important: convert object to string before returning to C#");
            sb.AppendLine("     return JSON.stringify(person);");
            sb.AppendLine("}");
            sb.AppendLine("tempFunction();");

            var task = ChromeUtils.m_chromeBrowser.EvaluateScriptAsync(sb.ToString());

            task.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    // Step 04: Recieve value from JS
                    var response = t.Result;

                    if (response.Success == true)
                    {
                        // Use JSON.net to convert to object;
                        MessageBox.Show(response.Result.ToString());
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenPage();
        }
        private void OpenPage()
        {
            panel1.Controls.Remove(ChromeUtils.m_chromeBrowser);

            var page = new Uri(string.Format("file:///{0}HTMLResources/html/ball/fc.html", GetAppLocation()));

            ChromeUtils.m_chromeBrowser = new ChromiumWebBrowser(page.ToString());

            panel1.Controls.Add(ChromeUtils.m_chromeBrowser);

            var m_jsInteractionObj2 = new BaseJsObj();
            // Register the JavaScriptInteractionObj class with JS
            ChromeUtils.m_chromeBrowser.RegisterJsObject("winformObj", m_jsInteractionObj2, false);
        }
    }
}
