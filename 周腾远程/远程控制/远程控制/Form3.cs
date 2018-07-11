using Manager;
using MSTSCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 远程控制
{
    public partial class Form3 : Form
    {

        public Form3(RemoteMachine remoteMachine)
        {
            InitializeComponent();
            rdp.Width = this.Size.Width;
            rdp.Height = this.Size.Height;
            rdp.Server = remoteMachine.Server;
            rdp.UserName = remoteMachine.UserName;
            IMsTscNonScriptable secured = (IMsTscNonScriptable)rdp.GetOcx();
            secured.ClearTextPassword = remoteMachine.Password;
            rdp.Dock = DockStyle.Fill;
            this.Text = remoteMachine.DesktopName;
            //if (!string.IsNullOrEmpty(remoteMachine.Password))
            //rdp.AdvancedSettings5.ClearTextPassword = remoteMachine.Password;
            rdp.AdvancedSettings5.RedirectDrives = remoteMachine.RedirectDrives;
            rdp.AdvancedSettings5.RedirectPrinters = remoteMachine.RedirectPrinters;
            rdp.AdvancedSettings5.RedirectPorts = remoteMachine.RedirectPorts;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            rdp.Connect();
            
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                rdp.Disconnect();
            }
            catch (Exception ex)
            {
            }
        }

        private void Form3_SizeChanged(object sender, EventArgs e)
        {
        }
    }
}
