using ChromeTest.Bind;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeTest
{
    public partial class frmBind : Form
    {
        public frmBind()
        {
            InitializeComponent();
        }

        private void frmBind_Load(object sender, EventArgs e)
        {
            var model = new TestModel { Value = 50, BtnName = "绑定事件" };
            new ViewBind(this, model);
        }
    }


    public class TestModel
    {
        List<Dep> dep = Dep.InitDeps(3);
        public int Value { get => dep[0].Get<int>(); set => dep[0].Set(value); }
        public string Text { get => dep[1].Get<string>(); set => dep[1].Set(value); }
        public string BtnName { get => dep[2].Get<string>(); set => dep[2].Set(value); }

        public void BtnClick(object o)
        {
            this.BtnName = string.Format("绑定事件{0}", DateTime.Now.Millisecond);
        }
    } 
}
