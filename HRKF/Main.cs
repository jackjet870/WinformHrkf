
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
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
    public partial class Main : Form
    {
        string Visitorse = string.Empty;
        public Main(string admin1)
        {
            InitializeComponent();
            admin = admin1;
            ConnectAsync();
           
            
        }
        public string admin = string.Empty;
        string free = string.Empty;
        string Visitor = string.Empty;
        HubConnection conn = new HubConnection("http://localhost:50076/signalr/hubs");
        IHubProxy ihubProxy;
        private void VisitorList()
        {
            string selectVisitor = "select * from users where username='" + admin + "'";
            SqlDataReader drVisitor = SqlHelper.GetReader(selectVisitor, CommandType.Text);
            while (drVisitor.Read())
            {
                Visitorse = drVisitor["FromUser"].ToString();
            }
            drVisitor.Close();
            string[] str = Visitorse.Split(',');
            for (int i = 0; i < str.Count(); i++)
            {

                treeView1.Nodes[1].Nodes.Add(str[i]);
                //this.treeView1.jiedian2.Nodes.Add(str[i]);
            }
        }
        private async void ConnectAsync()
        {

            //VisitorList();
            conn = new HubConnection("http://localhost:50076/signalr/hubs");
            ihubProxy = conn.CreateHubProxy("messageHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            ihubProxy.On<string, string>("AAA", (name, message) => this.Invoke((Action)(() =>
                     richTextBox1.AppendText(String.Format("{0}: {1}" + Environment.NewLine, name, message))
                //treeView1.Nodes[1].Nodes.Add(name)
                //MessageBox.Show(""))

                ))
               
               
            );
            ihubProxy.On<string>("AA", (name) => this.Invoke((Action)(() =>
                   // richTextBox1.AppendText(String.Format("{0}: {1}" + Environment.NewLine, name, message))
               treeView1.Nodes[1].Nodes.Add(name)
               //MessageBox.Show(""))

               ))


           );
            try
            {
                await conn.Start();
                //string select = "select FromUser from users where username='"+admin+"'";
                //SqlDataReader dr = SqlHelper.GetReader(select, CommandType.Text);
                //while (dr.Read())
                //{
                    
                //}

            }
            catch (Exception)
            {
                free = "free";
                richTextBox1.AppendText("目前没有客户" + Environment.NewLine);
            }
            
            //Activate UI

            textBox1.Focus();
           // richTextBox1.AppendText("Connected to server at " + "http://localhost:50076/signalr/hubs" + Environment.NewLine);
            //UserManager.AddUser("bbb", conn.ConnectionId);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                ihubProxy.Invoke("Send", admin, Visitor, htmlEditor1.BodyInnerText);
                richTextBox1.AppendText(String.Format("{0}" + Environment.NewLine, "我：" + htmlEditor1.BodyInnerText));
                htmlEditor1.BodyInnerText = "";               
            }
            catch
            {
                if (free=="free")
                {
                    MessageBox.Show("目前没有客户");
                }
                else
                {
                     MessageBox.Show("你处于离线状态");
                }
               
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            string state = "update users set state='离线',FromUser='',count=0 where username='" + admin + "'";
            SqlHelper.ExeSql(state, CommandType.Text);
            conn.Stop();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        private void htmlEditor1_Load(object sender, EventArgs e)
        {
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string state = "update users set state='" + comboBox1.Text + "',FromUser='',count=0 where username='" + admin + "'";
            //SqlHelper.ExeSql(state, CommandType.Text);
            //if (comboBox1.Text == "离线")
            //{
            //    button1.Enabled = false;
            //    conn.Stop();
            //}
            //else
            //{
            //    button1.Enabled = true;
            //    ConnectAsync();
            //}

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
          
            if (treeView1.SelectedNode.Text!="排队中的访客"&& treeView1.SelectedNode.Text != "浏览网页的访客" && treeView1.SelectedNode.Text != "自己" && treeView1.SelectedNode.Text != "已离开的访客")
            {
                button1.Enabled = true;
                Visitor = treeView1.SelectedNode.Text;
            }
            else
            {
                button1.Enabled = false;
            }


        }
    }
}
