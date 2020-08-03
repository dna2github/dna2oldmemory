using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PulseWaveAnalyzer.model;

namespace PulseWaveAnalyzer
{
    public partial class frmMain : Form
    {
        public const string version = "J.Y.Liu 2012";
        private ColorGradient color = new ColorGradient(new SpacePoint[] {
            new SpacePoint(0.0,0.0,1.0,0.0),
            new SpacePoint(0.0,1.0,0.0,0.3),
            new SpacePoint(1.0,1.0,0.0,0.5),
            new SpacePoint(1.0,0.0,0.0,1.0)
        });
        private PulseWave pw;

        private Bitmap viewbmp;
        private int viewcur, viewcnt, viewgroup;

        private Bitmap fftbmp;
        private int fftcur, fftcnt;

        private void drawView()
        {
            if (viewbmp == null)
                viewbmp = new Bitmap(picView.Width, picView.Height);
            Graphics g = Graphics.FromImage(viewbmp);
            g.Clear(Color.White);

            double tmp = 0.0;
            double smin = pw.grmin * 0.8;
            for (int i = 0; i < picView.Width; i++)
            {
                if (i + viewcur >= viewcnt) break;
                tmp = (pw.gr[i + viewcur] - smin) / (pw.grmax - smin);
                g.DrawLine(new Pen(color.select(tmp)), i, picView.Height, i, (int)((1 - tmp) * picView.Height));
                //g.DrawLine(new Pen(color.select(tmp)), i, 0, i, picView.Height);
            }

            picView.Image = viewbmp;

            if (fftbmp == null)
                fftbmp = new Bitmap(picF.Width, picF.Height);
            g = Graphics.FromImage(fftbmp);
            g.Clear(Color.White);

            tmp = 0.0;
            smin = pw.fftmin * 0.8;
            int nn = 2;
            for (int i = 0; i < picF.Width; i+=nn)
            {
                if (i/nn + fftcur >= fftcnt) break;
                tmp = (pw.rawfft[i/nn + fftcur] - smin) / (pw.fftmax - smin);
                g.FillRectangle(new SolidBrush(color.select(tmp)), i, (int)((1 - tmp) * picF.Height), nn, (int)(tmp * picF.Height));
                //g.DrawLine(new Pen(color.select(tmp)), i, picF.Height, i, (int)((1 - tmp) * picF.Height));
            }

            picF.Image = fftbmp;
        }

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x020A /*WM_MOUSEWHEEL*/:
                    bool mousewheel_inc = ((int)m.WParam > 0);
                    // button: 0=empty, 1=left, 2=right, 3=left&right
                    From_MouseWheel((int)m.WParam & 0x3, mousewheel_inc);
                    break;
            }
            base.WndProc(ref m);
        }
        private void From_MouseWheel(int button, bool inc)
        {
            if (viewbmp == null) return;
            if (pw.gr == null) return;
            switch (button)
            {
                case 0:
                    if (inc)
                    {
                        viewcur += 10;
                        if (viewcur + picView.Width >= viewcnt)
                            viewcur = viewcnt - picView.Width;
                        drawView();
                    }
                    else
                    {
                        viewcur -= 10;
                        if (viewcur <= 0) viewcur = 0;
                        drawView();
                    }
                    break;
                case 2:
                    if (inc)
                    {
                        if (viewgroup >= 101) return;
                        viewgroup += 10;
                        viewcur = 0;
                        pw.Group(pw.raw.Length / viewgroup);
                        drawView();
                    }
                    else
                    {
                        if (viewgroup <= 1) return;
                        viewgroup -= 10;
                        viewcur = 0;
                        pw.Group(pw.raw.Length / viewgroup);
                        drawView();
                    }
                    break;
            }
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pw = new PulseWave();
                System.GC.Collect();
                pw.Load(dlg.FileName);
                pw.FourierTransform();
                fftcur = 0;
                fftcnt = 1024;

                viewgroup = 11;
                pw.Group(pw.raw.Length / viewgroup);

                viewcur = 0;
                viewcnt = pw.gr.Length;
                drawView();

                double[] m = pw.getF(0.8, 1.5, 50000, 1830);
                //txtResult.Text = "标准值: [心]100.00 [肝]80.38 [肾]53.67 [脾]41.03 [肺]22.52 [胃]17.64 [胆]12.48 [膀胱]6.94 [大肠]3.96 [三焦]2.63 [小肠]1.79 [心包]1.18";
                //txtResult.Text += "\r\n";
                txtResult.Text = "";
                txtResult.Text += "[状态]\r\n";
                txtResult.Text += string.Format("[　心]{0:F} \r\n", m[0], (m[0] - BodyCondition.bodyS[0]) / BodyCondition.bodyS[0] * 100);
                txtResult.Text += string.Format("[　肝]{0:F} ({1:F}%)\r\n", m[1], (m[1] - BodyCondition.bodyS[1]) / BodyCondition.bodyS[1] * 100);
                txtResult.Text += string.Format("[　肾]{0:F} ({1:F}%)\r\n", m[2], (m[2] - BodyCondition.bodyS[2]) / BodyCondition.bodyS[2] * 100);
                txtResult.Text += string.Format("[　脾]{0:F} ({1:F}%)\r\n", m[3], (m[3] - BodyCondition.bodyS[3]) / BodyCondition.bodyS[3] * 100);
                txtResult.Text += string.Format("[　肺]{0:F} ({1:F}%)\r\n", m[4], (m[4] - BodyCondition.bodyS[4]) / BodyCondition.bodyS[4] * 100);
                txtResult.Text += string.Format("[　胃]{0:F} ({1:F}%)\r\n", m[5], (m[5] - BodyCondition.bodyS[5]) / BodyCondition.bodyS[5] * 100);
                txtResult.Text += string.Format("[　胆]{0:F} ({1:F}%)\r\n", m[6], (m[6] - BodyCondition.bodyS[6]) / BodyCondition.bodyS[6] * 100);
                txtResult.Text += string.Format("[膀胱]{0:F} ({1:F}%)\r\n", m[7], (m[7] - BodyCondition.bodyS[7]) / BodyCondition.bodyS[7] * 100);
                txtResult.Text += string.Format("[大肠]{0:F} ({1:F}%)\r\n", m[8], (m[8] - BodyCondition.bodyS[8]) / BodyCondition.bodyS[8] * 100);
                txtResult.Text += string.Format("[三焦]{0:F} ({1:F}%)\r\n", m[9], (m[9] - BodyCondition.bodyS[9]) / BodyCondition.bodyS[9] * 100);
                txtResult.Text += string.Format("[小肠]{0:F} ({1:F}%)\r\n", m[10], (m[10] - BodyCondition.bodyS[10]) / BodyCondition.bodyS[10] * 100);
                txtResult.Text += string.Format("[心包]{0:F} ({1:F}%)\r\n", m[11], (m[11] - BodyCondition.bodyS[11]) / BodyCondition.bodyS[11] * 100);
                txtResult.Text += "\r\n";
                txtResult.Text += "[治疗]\r\n";
                txtResult.Text += getMedicineResult(m);
            }
        }

        private string getMedicineResult(double[] bodye)
        {
            double[] ef, efm, e;
            double s, sm;
            List<Prescript> result = new List<Prescript>();
            BodyCondition body = new BodyCondition();

            body.bodyM = bodye;
            ef = body.bodyconditionE();
            s = body.scoreBodyCondition(ef);
            foreach (Prescript x in Database.prescriptDB)
            {
                e = x.prescriptE();
                efm = body.eatmedicineE(e);
                sm = body.scoreBodyCondition(efm);
                if (sm < s)
                {
                    x.extra = true;
                    x.extrascore = sm;
                    result.Add(x);
                }
            }
            result.Sort();

            String tmp = "";
            foreach (Prescript x in result)
            {
                tmp += string.Format("[评分]{0,-10:F}   [方剂]" + x.name + "\r\n",
                    ((s - x.extrascore) / s * 100));
            }
            return tmp;
        }

        private void menuLibrary_Click(object sender, EventArgs e)
        {
            frmLibrary dialog = new frmLibrary();
            dialog.ShowDialog();
        }

        private void picView_MouseDown(object sender, MouseEventArgs e)
        {
            bool inc;
            int x = e.X;
            if (x < picView.Width / 20) inc = false;
            else if (x < picView.Width * 18 / 20) return;
            else inc = true;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    From_MouseWheel(0, inc);
                    break;
                case MouseButtons.Right:
                    From_MouseWheel(2, inc);
                    break;
            }
        }
    }
}