using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yiguanjia
{
    public partial class frmMain : Form, DataCollector.FutureData
    {
        public void process(byte[] buf, int N)
        {
            byte[] data = new byte[N];
            Array.Copy(buf, data, N);
            //txtTmp.Invoke(new MethodInvoker(delegate {
            //    txtTmp.Text = BitConverter.ToString(data);
            //}));
            this.Invoke(new Action(delegate {
                switch (this._cmd)
                {
                    case "03":
                        for (int i = 1, n = data.Length; i < n; i += 4)
                        {
                            // 80 X 16 Y
                            byte d = data[i];
                            if (d > data03[1]) data03[1] = d;
                            if (d > 0 && d < data03[2]) data03[2] = d;
                            else if (d > 0 && data03[2] < 0) data03[2] = d;
                            data03[3] = (int)(0.9 * data03[3] + 0.1 * d);
                            double R = 0.0;
                            if (d == 0) R = 0; else R = 5.0 / data03[3] * 100000;
                            btnAccuPointValue.Text = String.Format("Max:{0} Min:{1} Avg:{2} [{3:F2} kOmega]", data03[1], data03[2], data03[3], R);
                        }
                        break;
                    default:
                        txtTmp.Text = BitConverter.ToString(data);
                        break;
                }
            }));
        }

        DataCollector _dc;
        bool _portOpen = false;
        string _cmd = "00";

        // 03 [count, max, non-0 min, avg]
        int[] data03 = new int[] { 0, -1, -1, 0 };

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this._dc = new DataCollector(this);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!_portOpen)
            {
                this._dc.Connnect(txtComName.Text);
                this._portOpen = true;
                this.btnConnect.Text = "Disconnect";
            }
            else
            {
                this._dc.Disconnect();
                this._portOpen = false;
                this.btnConnect.Text = "Connect";
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_portOpen)
            {
                this._dc.Send(new byte[] { 0xC0, 0x00, 0x00, 0x00 });
                this._dc.Disconnect();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!this._portOpen) return;
            string cmd = txtCmd.Text.Split(' ')[0];
            this._cmd = cmd;
            switch (cmd)
            {
                case "00":
                    this._dc.Send(new byte[] { 0xC0, 0x00, 0x00, 0x00 });
                    break;
                case "03":
                    this._dc.Send(new byte[] { 0xC0, 0x03 });
                    tabData.SelectTab(tabAccupoint);
                    break;
                case "04":
                    this._dc.Send(new byte[] { 0xC0, 0x04 });
                    break;
                case "05":
                    this._dc.Send(new byte[] { 0xC0, 0x05 });
                    break;
                case "09":
                    this._dc.Send(new byte[] { 0xC0, 0x09 });
                    break;
                case "0C":
                    this._dc.Send(new byte[] { 0xC0, 0x0C });
                    tabData.SelectTab(tabPulsewave);
                    break;
            }
        }

        private void btnAccuPointValue_Click(object sender, EventArgs e)
        {
            data03[0] = 0;
            data03[1] = -1;
            data03[2] = -1;
            data03[3] = 0;
            btnAccuPointValue.Text = "";
        }
    }
}
