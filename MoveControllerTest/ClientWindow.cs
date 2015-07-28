using MoveController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MoveControllerTest
{
    public partial class ClientWindow : Form
    {
        ProxyClient _Client;
        public ClientWindow()
        {
            
            InitializeComponent();
            Debug.From = "Client";
            _Client = new ProxyClient(); 
            _Client.Init();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PositionLabel.Text = PSMove.RawPosition.ToString();
            OrientationLabel.Text = PSMove.RawOrientation.ToString();
        }
    }
}
