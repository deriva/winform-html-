using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeTest.Bind
{
    /// <summary>
    /// 监听者
    /// </summary>
    public class Watcher : IWatcher
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        private Type type = null;
        /// <summary>
        /// 属性变化触发的委托
        /// </summary>
        private Action<object> Action = null;
        /// <summary>
        /// 属性名称
        /// </summary>
        private string propertyName = null;
        /// <summary>
        /// 父控件
        /// </summary>
        private Control ParentControl = null;
        /// <summary>
        /// 实体
        /// </summary>
        private object model = null;
        /// <summary>
        /// 初始化监听者
        /// </summary>
        /// <param name="ParentControl">父控件</param>
        /// <param name="type">实体类型</param>
        /// <param name="model">实体</param>
        /// <param name="propertyName">要监听的属性名称</param>
        /// <param name="action">属性变化触发的委托</param>
        public Watcher(Control ParentControl, Type type, object model, string propertyName, Action<object> action)
        {
            this.type = type;
            this.Action = action;
            this.propertyName = propertyName;
            this.ParentControl = ParentControl;
            this.model = model;
            this.AddToDep();
        }
        /// <summary>
        /// 添加监听者到属性的订阅者列表（Dep）
        /// </summary>
        private void AddToDep()
        {
            PropertyInfo property = this.type.GetProperty(this.propertyName);
            if (property == null) return;
            Dep.Target = this;
            object value = property.GetValue(this.model, null);
            Dep.Target = null;
        }
        /// <summary>
        /// 更新数据（监听触发的方法）
        /// </summary>
        public void Update()
        {
            this.ParentControl.Invoke(new Action(delegate
            {
                this.Action(this.model);
            }));
        }
    }
}