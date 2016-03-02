using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace ZC
{
    public partial class zc : Form
    {
        public zc()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Validate())
            {
                return;
            }
            string sql = "insert user select username=@a,pwd=@b";
            SqlParameter[] pa = {
                new SqlParameter("@a",textBox1.Text),
                 new SqlParameter("@b",textBox2.Text)
            };
            int count=SqlHelper.ExeSql(sql,CommandType.Text,pa);
            if (count>0)
            {
                MessageBox.Show("恭喜，注册成功");
            }
            else
            {
                MessageBox.Show("失败");
            }

        }
        private bool Validate()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("账号不能为空");
                return false;
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("密码不能为空");
                return false;
            }
            else if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("确认密码不能为空");
                return false;
            }
            else if (textBox2.Text!=textBox3.Text)
            {
                MessageBox.Show("请保持密码一致");
                return false;
            }
            return true;
        }
        
    }
}
