using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChromeTest
{

    public class Person
    {
        public Person(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = birthDate;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int SkillLevel { get; set; }
    }

    public class JavaScriptInteractionObj
    {
        public Person m_theMan = null;

        [JavascriptIgnore]
        public ChromiumWebBrowser m_chromeBrowser { get; set; }

        public JavaScriptInteractionObj()
        {
            m_theMan = new Person("Bat", "Man", DateTime.Now);
        }

        [JavascriptIgnore]
        public void SetChromeBrowser(ChromiumWebBrowser b)
        {
            m_chromeBrowser = b;
        }

        public string SomeFunction()
        {
            return "yippieee";
        }

        public string GetPerson()
        {
            var p1 = new Person("Bruce", "Banner", DateTime.Now);

            string json = JsonConvert.SerializeObject(p1);

            return json;
        }

        public string ErrorFunction()
        {
            return null;
        }

        public string GetListOfPeople()
        {
            List<Person> peopleList = new List<Person>();

            peopleList.Add(new Person("Scooby", "Doo", DateTime.Now));
            peopleList.Add(new Person("Buggs", "Bunny", DateTime.Now));
            peopleList.Add(new Person("Daffy", "Duck", DateTime.Now));
            peopleList.Add(new Person("Fred", "Flinstone", DateTime.Now));
            peopleList.Add(new Person("Iron", "Man", DateTime.Now));

            string json = JsonConvert.SerializeObject(peopleList);

            return json;
        }

        public void ExecJSFromWinForms()
        {
            var script = "document.body.style.backgroundColor = 'red';";

            m_chromeBrowser.ExecuteScriptAsync(script);
        }

        public void TestJSCallback(IJavascriptCallback javascriptCallback)
        {
            using (javascriptCallback)
            {
                javascriptCallback.ExecuteAsync("Hello from winforms and C# land!");
            }
        }
    }
    public class Table2JJObj : BaseJsObj
    {

        public Table2JJObj()
        {

        }
        public string AddData3()
        {
            var jo = new JObject();
            jo["Name"] = "12";
            jo["Name1"] = "122";
            jo["Name2"] = "1232";
            jo["Name3"] = "144442";
            return jo.ToString();
        }

        public void AddData2(string obj)
        {
            var jo = new JObject();
            jo["Name"] = "12";
            jo["Name1"] = "122";
            jo["Name2"] = "1232";
            jo["Name3"] = "144442";
            ChromeUtils.m_chromeBrowser.ExecuteScriptAsync("AddData('" + Newtonsoft.Json.JsonConvert.SerializeObject(jo) + "')");
        }

        public void ExecJSFromWinForms()
        {
            var script = "document.body.style.backgroundColor = 'red';";

            ChromeUtils.m_chromeBrowser.ExecuteScriptAsync(script);
        }

        public void TestJSCallback(IJavascriptCallback javascriptCallback)
        {
            using (javascriptCallback)
            {
                javascriptCallback.ExecuteAsync("Hello from winforms and C# land!");
            }
        }
    }

    public class JJJObj
    {


        public string GetDataList()
        {
            var jo = new JObject();
            jo["Name"] = "ee12";
            jo["Name1"] = "1eee22";
            jo["Name2"] = "12ee32";
            jo["Name3"] = "14ee4442";
            return jo.ToString();
        }

        public void ReaderList(object obj)
        {
            var jo = new JObject();
            jo["Name"] = "12";
            jo["Name1"] = "122";
            jo["Name2"] = "1232";
            jo["Name3"] = "144442";
            ChromeUtils.m_chromeBrowser.ExecuteScriptAsync("AddData('" + Newtonsoft.Json.JsonConvert.SerializeObject(jo) + "')");
        }

        public void ExecJSFromWinForms()
        {
            var script = "document.body.style.backgroundColor = 'red';";

            ChromeUtils.m_chromeBrowser.ExecuteScriptAsync(script);
        }

        public void TestJSCallback(IJavascriptCallback javascriptCallback)
        {
            using (javascriptCallback)
            {
                javascriptCallback.ExecuteAsync("Hello from winforms and C# land!");
            }
        }


    }
     
    public class ResultHelper
    {
        public static ResponseResult ToResult(int code, string msg = "", object data = null)
        {
            return new ResponseResult() { Code = code, Msg = msg, Data = data };
        }
        public static ResponseResult ToSuccess(string msg = "", object data = null)
        {
            return new ResponseResult() { Code = 100, Msg = msg, Data = data };
        }
        public static ResponseResult ToFail(string msg = "", object data = null)
        {
            return new ResponseResult() { Code = 101, Msg = msg, Data = data };
        }
    }


    public class ResponseResult
    {
        /// <summary>
        /// 响应码 ：100 成功   101失败
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 响应的数据
        /// </summary>
        public object Data { get; set; }
    }
}