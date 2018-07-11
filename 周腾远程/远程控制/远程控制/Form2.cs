using Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 远程控制
{
    public partial class Form2 : Form
    {
        public Form2(RemoteMachine remoteMachine)
        {
            InitializeComponent();
            Loadcombobox();
            textBox1.Text = remoteMachine.DesktopName;
            textBox1.Enabled = false;
            textBox2.Text = remoteMachine.WWW;
            textBox3.Text = remoteMachine.Server;
            textBox4.Text = remoteMachine.UserName;
            textBox5.Text = remoteMachine.Password;
            comboBox1.Text = remoteMachine.Type;
        }
        public Form2()
        {
            InitializeComponent();
            Loadcombobox();
            comboBox1.Text = "默认";
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Loadcombobox();
                IniFile iniFile = new IniFile(Path.Combine(Application.StartupPath, "config.ini"));
                RemoteMachine remoteMachine = new RemoteMachine();
                remoteMachine.DesktopName = textBox1.Text;
                remoteMachine.WWW = textBox2.Text;
                remoteMachine.Server = textBox3.Text;
                remoteMachine.UserName = textBox4.Text;
                remoteMachine.Password = textBox5.Text;
                remoteMachine.Type = comboBox1.Text;
                RemoteMachine.Save(remoteMachine, iniFile);
                MessageBox.Show("操作成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void Loadcombobox()
        {
            string a = ConfigurationManager.AppSettings.Get("type");
            comboBox1.Items.Add("默认");
            string[] type = a.Split(',');
            foreach (string s in type)
            {
                comboBox1.Items.Add(s);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
