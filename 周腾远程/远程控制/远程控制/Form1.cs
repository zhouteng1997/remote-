using Manager;
using MSTSCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace 远程控制
{
    public partial class Form1 : Form
    {
        private IniFile ini;
        private string inipath;
        private List<RemoteMachine> macs;
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体运行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //复选框加载默认数据
            LoadcomboBoxType();
            comboBoxType.Text = "默认";
            //dataGridView1数据显示
            LoadIni(Path.Combine(Application.StartupPath, "config.ini"));
        }

        /// <summary>
        /// 获取配置文件中数据，并将其放置入复选框中
        /// </summary>
        public  void LoadcomboBoxType()
        {
            string a = ConfigurationManager.AppSettings.Get("type");
            comboBoxType.Items.Add("默认");
            string[] type = a.Split(',');
            foreach(string s in type)
            {
               comboBoxType.Items.Add(s);
            }
        }

        /// <summary>
        /// 连接按钮   连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            #region  原本的代码,初始连接尝试----通过----逻辑无错误
            //if (Lj.Text == "连接")
            //{
            //    string ip= "118.24.20.161";
            //    string user = "Administrator";
            //    string pwd = "2018";

            //    try
            //    {
            //        rdp.Server = ip;
            //        rdp.UserName = user;
            //        IMsTscNonScriptable secured = (IMsTscNonScriptable)rdp.GetOcx();
            //        secured.ClearTextPassword = pwd;
            //        rdp.Dock = DockStyle.Fill;
            //        rdp.Connect();
            //        Lj.Text = "断开连接";
            //        Lj.BackColor = Color.Red;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //        return;
            //    }
            //}
            //else
            //{
            //    Lj.Text = "连接";
            //    Lj.BackColor = Color.PaleGreen;
            //    try
            //    {
            //        rdp.Disconnect();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("断开连接失败:" + ex.Message);
            //        return;
            //    }

            //}
            #endregion
            RemoteMachine mac = RemoteMachineGetRows();
            try
            {
                Form3 df = new Form3(mac);
                df.Show();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 绑定数据，并显示
        /// </summary>
        /// <param name="path"></param>
        private void LoadIni(string path)
        {
            //清除多余的信息
            dataGridView1.AutoGenerateColumns = false;
            //绑定数据源
            dataGridView1.DataSource = Sourcebind(path);
           
        }

        /// <summary>
        /// 数据源获取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<RemoteMachine> Sourcebind(string path)
        {
            string type = comboBoxType.Text;
            inipath = path;
            ini = new IniFile(path);
            macs = RemoteMachine.Load(ini,type,"1");
            return macs;
        }

        /// <summary>
        /// 修改选中行的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                RemoteMachine remoteMachine = RemoteMachineGetRows();
                using (Form2 form2 = new Form2(remoteMachine))
                {
                    if (form2.ShowDialog() == DialogResult.OK)
                    {
                        LoadIni(Path.Combine(Application.StartupPath, "config.ini"));
                    }
                }
            }
            catch
            {
                MessageBox.Show("请选择操作行");
            }
        }

        /// <summary>
        /// 获取选中行的信息
        /// </summary>
        /// <returns></returns>
        private RemoteMachine RemoteMachineGetRows()
        {
            RemoteMachine remoteMachine = new RemoteMachine();
            int z = dataGridView1.CurrentRow.Index;
            remoteMachine.DesktopName = dataGridView1.Rows[z].Cells["服务器"].Value.ToString();
            remoteMachine.WWW = dataGridView1.Rows[z].Cells["备注"].Value.ToString();
            remoteMachine.Server = dataGridView1.Rows[z].Cells["ip"].Value.ToString();
            remoteMachine.UserName = dataGridView1.Rows[z].Cells["账号"].Value.ToString();
            remoteMachine.Password = dataGridView1.Rows[z].Cells["密码"].Value.ToString();
            remoteMachine.Type = dataGridView1.Rows[z].Cells["类型"].Value.ToString();
            //共享磁盘
            if (checkBox1.Checked == true) remoteMachine.RedirectDrives = true;
            //共享打印机
            if (checkBox2.Checked == true) remoteMachine.RedirectPrinters = true;
            //共享端口
            if (checkBox3.Checked == true) remoteMachine.RedirectPorts = true;
            return remoteMachine;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tj_Click(object sender, EventArgs e)
        {
            using (Form2 form = new Form2())
            {
                if( form.ShowDialog() == DialogResult.OK)
                {
                    LoadIni(Path.Combine(Application.StartupPath, "config.ini"));
                }
            }
        }

        /// <summary>
        /// 点击删除按钮触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sc_Click(object sender, EventArgs e)
        {
            try
            {
                RemoteMachine remoteMachine = RemoteMachineGetRows();
                if (MessageBox.Show(string.Format("确认删除\"{0}\"？", remoteMachine.DesktopName), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    remoteMachine.Delete(this.ini);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                LoadIni(Path.Combine(Application.StartupPath, "config.ini"));
            }
            catch
            {
                MessageBox.Show("请选择操作行");
            }

        }

        /// <summary>
        /// 双击数据框会触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            RemoteMachine mac = RemoteMachineGetRows();
            //共享磁盘
            if (checkBox1.Checked == true)
                mac.RedirectDrives = true;
            //共享打印机
            if (checkBox2.Checked == true)
                mac.RedirectPrinters = true;
            //共享端口
            if (checkBox3.Checked == true)
                mac.RedirectPorts = true;
            try
            {
                #region
                //using (Form3 df = new Form3(mac))
                //{
                //    //不关闭无法操作
                //    //df.ShowDialog();
                //    df.Show();
                //}
                #endregion
                Form3 df = new Form3(mac);
                df.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 下拉框选择改变时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIni(Path.Combine(Application.StartupPath, "config.ini"));
        }
    }
}

