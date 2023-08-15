using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeTest.Bind
{
    public class ViewBind
    {
        /// <summary>
        /// 默认绑定事件
        /// </summary>
        private string DefaultEvents = "CollectionChange|SelectedValueChanged|ValueChanged|TextChanged";
        /// <summary>
        /// 默认绑定的属性，从左往右，能找到则赋值
        /// </summary>
        private string DefaultProperty = "DataSource|Value|Text";


        /// <summary>
        /// 绑定视图
        /// </summary>
        /// <param name="ParentControl">父控件</param>
        /// <param name="model">模型（对象）</param>
        public ViewBind(Control ParentControl, object model)
        {
            this.BindingParentControl(ParentControl, model);
        }
        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="ParentControl">父控件</param>
        /// <param name="model">实体</param>
        private void BindingParentControl(Control ParentControl, object model)
        {
            this.BindControl(ParentControl, model, ParentControl.Controls);
        }
        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="ParentControl">父控件</param>
        /// <param name="model">实体</param>
        /// <param name="Controls">子控件列表</param>
        private void BindControl(Control ParentControl, object model, Control.ControlCollection Controls)
        {
            foreach (Control control in Controls)
            {
                var tag = control.Tag;

                if (tag == null) continue;
                foreach (var tagInfo in tag.ToString().Split('|'))
                {
                    var tagInfoArr = tagInfo.Split('-');
                    //属性绑定
                    if (tagInfoArr[0].Equals("dt") || tagInfoArr[0].Equals("data"))
                    {
                        var bindProperty = string.Empty;
                        if (tagInfoArr.Length == 2)
                        {
                            foreach (var property in DefaultProperty.Split('|'))
                            {
                                if (control.GetType().GetProperty(property) != null)
                                {
                                    bindProperty = property;
                                    break;
                                }
                            }
                        }
                        else if (tagInfoArr.Length == 3)
                        {
                            bindProperty = tagInfoArr[1];
                        }
                        else continue;

                        string propertyName = tagInfoArr[tagInfoArr.Length - 1];
                        this.BindingProperty(ParentControl, control, model, propertyName, bindProperty);
                        this.BindListener(control, model, propertyName, bindProperty);
                    }
                    else if (tagInfoArr[0].Equals("ev") && tagInfoArr.Length == 3)
                    {
                        //事件绑定
                        BindEvent(ParentControl, control, model, tagInfoArr[1], tagInfoArr[2]);
                    }
                    else
                    {
                        if (control.Controls.Count > 0)
                        {
                            this.BindControl(ParentControl, model, control.Controls);
                        }
                    }
                }

            }
        }
        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="ParentControl">父控件</param>
        /// <param name="control">要绑定事件的控件</param>
        /// <param name="model">实体</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="methodName">绑定到的方法</param>
        private void BindEvent(Control ParentControl, Control control, object model, string eventName, string methodName)
        {
            var Event = control.GetType().GetEvent(eventName);
            if (Event != null)
            {
                var methodInfo = model.GetType().GetMethod(methodName);
                if (methodInfo != null)
                {
                    Event.AddEventHandler(control, new EventHandler((s, e) =>
                    {
                        ParentControl.Invoke(new Action(() =>
                        {
                            switch (methodInfo.GetParameters().Count())
                            {
                                case 0: methodInfo.Invoke(model, null); break;
                                case 1: methodInfo.Invoke(model, new object[] { new { s = s, e = e } }); break;
                                case 2: methodInfo.Invoke(model, new object[] { s, e }); break;
                                default: break;
                            }
                        }));
                    }));
                }
            }
        }
        /// <summary>
        /// 添加监听事件
        /// </summary>
        /// <param name="control">要监听的控件</param>
        /// <param name="model">实体</param>
        /// <param name="mPropertyName">变化的实体属性</param>
        /// <param name="controlPropertyName">对应变化的控件属性</param>
        private void BindListener(Control control, object model, string mPropertyName, string controlPropertyName)
        {
            var property = model.GetType().GetProperty(mPropertyName);

            var events = this.DefaultEvents.Split('|');
            foreach (var ev in events)
            {
                var Event = control.GetType().GetEvent(ev);
                if (Event != null)
                {
                    Event.AddEventHandler(control, new EventHandler((s, e) =>
                    {
                        try
                        {
                            var controlProperty = control.GetType().GetProperty(controlPropertyName);
                            if (controlProperty != null)
                            {
                                property.SetValue(model, Convert.ChangeType(controlProperty.GetValue(control, null), property.PropertyType), null);

                            }
                        }
                        catch (Exception ex)
                        {
                            //TPDO
                        }
                    }));
                }
            }
        }
        /// <summary>
        /// 绑定属性
        /// </summary>
        /// <param name="ParentControl">父控件</param>
        /// <param name="control">绑定属性的控件</param>
        /// <param name="model">实体</param>
        /// <param name="mPropertyName">绑定的实体属性名称</param>
        /// <param name="controlPropertyName">绑定到的控件的属性名称</param>
        private void BindingProperty(Control ParentControl, Control control, object model, string mPropertyName, string controlPropertyName)
        {

            Action<object> action = m =>
            {
                try
                {
                    var controlType = control.GetType();
                    var mType = m.GetType().GetProperty(mPropertyName);
                    var controlProperty = controlType.GetProperty(controlPropertyName);
                    if (controlProperty != null)
                    {
                        switch (controlPropertyName)
                        {
                            case "DataSource":
                                controlProperty.SetValue(control, mType.GetValue(m, null), null);
                                break;
                            default:
                                controlProperty.SetValue(control, Convert.ChangeType(mType.GetValue(m, null), controlProperty.PropertyType), null);
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    //TODO
                }
            };
            //添加到监听
            new Watcher(ParentControl, model.GetType(), model, mPropertyName, action);
            //初始化数据（将实体数据赋给控件属性）
            action(model);
        }
    }

}
