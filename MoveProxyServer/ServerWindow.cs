using MoveController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoveProxyServer
{
    public partial class ServerWindow : Form
    {
        ProxyServer _Server;
        public ServerWindow()
        {
            InitializeComponent();
            Debug.From = "Server";
            _Server = new ProxyServer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            _Server.Init();   
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ConnectionStatusLabel.Text = _Server.IsConnected ? "Connected" : "Not Connected";
        }
    }
}
