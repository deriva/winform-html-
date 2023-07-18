# WinFormsHTMLChromium
winform+html+谷歌浏览器实现交互

里面含有各种demo:表格，echart报表等等

1.首先要在winform里注册js对象给到html 
    var m_jsInteractionObj2 = new BaseJsObj(); 
    // Register the JavaScriptInteractionObj class with JS
    ChromeUtils.m_chromeBrowser.RegisterJsObject("winformObj", m_jsInteractionObj2, false);
    //winformObj就是winform注册给html的对象  相当于html页面的windows


2页面调用winform方法：
  `winformObj.Invoke("ChromeTest/JJJObj/ReaderList", {id:23});`


3.winform调用页面的js方法
 `ChromeUtils.m_chromeBrowser.ExecuteScriptAsync("AddData('" + Newtonsoft.Json.JsonConvert.SerializeObject(jo) + "')");`

