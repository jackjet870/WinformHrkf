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
namespace HRKF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "select count(0) from users where username=@a and pwd=@b";
            SqlParameter[]  pa = {
                               new SqlParameter("@a",textBox1.Text),
                                new SqlParameter("@b",textBox2.Text)
            };
           
            object ob= SqlHelper.GetOneData(sql, CommandType.Text, pa);
            if ((int)ob>0)
            {
                MessageBox.Show("登陆成功");
                string state = "update users set state='在线' where username='"+textBox1.Text+"'";
                SqlHelper.ExeSql(state,CommandType.Text);
                Main main = new Main(textBox1.Text);
              
                main.ShowDialog();
                this.Close();
                

            }
            else
            {
                MessageBox.Show("账号或者密码错误");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ZC zc = new ZC();
            zc.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
