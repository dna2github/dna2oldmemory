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
    public partial class frmLibrary : Form
    {
        private Medicine memM;
        private Prescript memP;
        private Bitmap bmpPicpVal;

        private double getMedicineMild()
        {
            if (rbNone.Checked) return 0.0;
            if (rbWarm.Checked) return 0.5;
            if (rbHot.Checked) return 1.0;
            if (rbCool.Checked) return -0.5;
            return -1.0;
        }
        private void setMedicineMild(double val)
        {
            if (val == 0.0) rbNone.Checked = true;
            else if (val == 0.5) rbWarm.Checked = true;
            else if (val == 1.0) rbHot.Checked = true;
            else if (val == -0.5) rbCool.Checked = true;
            else rbCold.Checked = true;
        }
        private void drawPrescriptValue()
        {
            double[] e = memP.prescriptE();
            int bw = picpVal.Width / e.Length;
            Graphics g = Graphics.FromImage(bmpPicpVal);
            Brush b;
            g.Clear(Color.White);
            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] == 0.0) b = Brushes.White;
                else if (e[i] > 0.0) b = Brushes.Red;
                else b = Brushes.Blue;
                g.FillRectangle(b, i * bw, 0, bw, (int)(Math.Abs(e[i]) * picpVal.Height));
            }
            picpVal.Image = bmpPicpVal;
        }

        public frmLibrary()
        {
            InitializeComponent();

            memM = new Medicine();
            memP = new Prescript();

            mlist.Items.Clear();
            plist.Items.Clear();
            pmlist.Items.Clear();
            pplist.Items.Clear();

            bmpPicpVal = new Bitmap(picpVal.Width, picpVal.Height);
            drawPrescriptValue();

            foreach (Medicine x in Database.medicineDB)
            {
                mlist.Items.Add(x.name);
                pmlist.Items.Add(x.name);
            }
            foreach (Prescript x in Database.prescriptDB)
            {
                plist.Items.Add(x.name);
            }
        }

        private void btnmNew_Click(object sender, EventArgs e)
        {
            mlist.ClearSelected();
            txtmName.Text = "";
            rbNone.Checked = true;
            //rbCold.Checked = false;
            //rbWarm.Checked = false;
            cbBL.Checked = false;
            cbGB.Checked = false;
            cbHT.Checked = false;
            cbKI.Checked = false;
            cbLI.Checked = false;
            cbLR.Checked = false;
            cbLU.Checked = false;
            cbPC.Checked = false;
            cbSI.Checked = false;
            cbSP.Checked = false;
            cbST.Checked = false;
            cbTB.Checked = false;
        }

        private void btnpNew_Click(object sender, EventArgs e)
        {
            plist.ClearSelected();
            txtpName.Text = "";
            pplist.Items.Clear();
            memP.clear();
            Graphics.FromImage(bmpPicpVal).Clear(Color.White);
            picpVal.Image = bmpPicpVal;
        }

        private void mlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mlist.SelectedIndex < 0) return;
            Medicine m = Database.medicineDB[mlist.SelectedIndex];
            txtmName.Text = m.name;
            setMedicineMild(m.mild);

            if (m.merdian[0] != 0.0) cbHT.Checked = true; else cbHT.Checked = false;
            if (m.merdian[1] != 0.0) cbLR.Checked = true; else cbLR.Checked = false;
            if (m.merdian[2] != 0.0) cbKI.Checked = true; else cbKI.Checked = false;
            if (m.merdian[3] != 0.0) cbSP.Checked = true; else cbSP.Checked = false;
            if (m.merdian[4] != 0.0) cbLU.Checked = true; else cbLU.Checked = false;
            if (m.merdian[5] != 0.0) cbST.Checked = true; else cbST.Checked = false;
            if (m.merdian[6] != 0.0) cbGB.Checked = true; else cbGB.Checked = false;
            if (m.merdian[7] != 0.0) cbBL.Checked = true; else cbBL.Checked = false;
            if (m.merdian[8] != 0.0) cbLI.Checked = true; else cbLI.Checked = false;
            if (m.merdian[9] != 0.0) cbTB.Checked = true; else cbTB.Checked = false;
            if (m.merdian[10] != 0.0) cbSI.Checked = true; else cbSI.Checked = false;
            if (m.merdian[11] != 0.0) cbPC.Checked = true; else cbPC.Checked = false;
        }

        private void plist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (plist.SelectedIndex < 0) return;
            Prescript p = Database.prescriptDB[plist.SelectedIndex];

            txtpName.Text = p.name;
            pplist.Items.Clear();
            memP.clear();
            foreach (PrescriptNode pn in p.getPrescriptMedicines())
            {
                pplist.Items.Add(string.Format("{0:F}g\t", pn.weight) + pn.med.name);
                memP.add(pn.med, pn.weight, pn.state);
            }
            drawPrescriptValue();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (mlist.SelectedIndex < 0)
            {
                btnmNew_Click(sender, e);
            }
            else
            {
                mlist_SelectedIndexChanged(sender, e);
            }
        }

        private void btnpCancel_Click(object sender, EventArgs e)
        {
            if (mlist.SelectedIndex < 0)
            {
                btnpNew_Click(sender, e);
            }
            else
            {
                plist_SelectedIndexChanged(sender, e);
            }
        }

        private void setMedicine(Medicine m)
        {
            if (m == null) return;
            m.name = txtmName.Text;
            m.mild = getMedicineMild();
            if (cbHT.Checked) m.merdian[0] = 1.0; else m.merdian[0] = 0.0;
            if (cbLR.Checked) m.merdian[1] = 1.0; else m.merdian[1] = 0.0;
            if (cbKI.Checked) m.merdian[2] = 1.0; else m.merdian[2] = 0.0;
            if (cbSP.Checked) m.merdian[3] = 1.0; else m.merdian[3] = 0.0;
            if (cbLU.Checked) m.merdian[4] = 1.0; else m.merdian[4] = 0.0;
            if (cbST.Checked) m.merdian[5] = 1.0; else m.merdian[5] = 0.0;
            if (cbGB.Checked) m.merdian[6] = 1.0; else m.merdian[6] = 0.0;
            if (cbBL.Checked) m.merdian[7] = 1.0; else m.merdian[7] = 0.0;
            if (cbLI.Checked) m.merdian[8] = 1.0; else m.merdian[8] = 0.0;
            if (cbTB.Checked) m.merdian[9] = 1.0; else m.merdian[9] = 0.0;
            if (cbSI.Checked) m.merdian[10] = 1.0; else m.merdian[10] = 0.0;
            if (cbPC.Checked) m.merdian[11] = 1.0; else m.merdian[11] = 0.0;
        }
        private void btnmOK_Click(object sender, EventArgs e)
        {
            Medicine m;
            if (mlist.SelectedIndex < 0)
            {
                m = new Medicine();
                m.id = -1;
                setMedicine(m);
                DBParser.addMedicine(Database.medicineDB, m);
                if (m.id > 0) Database.medicineDB.Add(m);
                mlist.Items.Add(m.name);
                //mlist.SelectedIndex = mlist.Items.Count - 1;
                pmlist.Items.Add(m.name);
                btnmNew_Click(sender, e);
            }
            else
            {
                m = Database.medicineDB[mlist.SelectedIndex];
                setMedicine(m);
                DBParser.updateMedicine(m);
                mlist.Items[mlist.SelectedIndex] = m.name;
                pmlist.Items[mlist.SelectedIndex] = m.name;
                mlist_SelectedIndexChanged(sender, e);
            }
            txtmName.Focus();
        }

        private void setPrescript(Prescript p)
        {
            if (p == null) return;
            p.name = txtpName.Text;
            p.clear();
            foreach (PrescriptNode x in memP.getPrescriptMedicines())
            {
                p.add(x.med, x.weight, x.state);
            }
        }
        private void btnpOK_Click(object sender, EventArgs e)
        {
            Prescript p;
            if (plist.SelectedIndex < 0)
            {
                p = new Prescript();
                p.id = -1;
                setPrescript(p);
                DBParser.addPrescript(Database.prescriptDB, p);
                if (p.id > 0) Database.prescriptDB.Add(p);
                plist.Items.Add(p.name);
                //plist.SelectedIndex = plist.Items.Count - 1;
                btnpNew_Click(sender, e);
            }
            else
            {
                p = Database.prescriptDB[plist.SelectedIndex];
                setPrescript(p);
                DBParser.updatePrescript(p);
                plist.Items[plist.SelectedIndex] = p.name;
                plist_SelectedIndexChanged(sender, e);
            }
            txtpName.Focus();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (pplist.SelectedIndex < 0) return;
            memP.del(memP.getPrescriptMedicines()[pplist.SelectedIndex].med);
            pplist.Items.RemoveAt(pplist.SelectedIndex);
            drawPrescriptValue();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (pmlist.SelectedIndex < 0) return;
            if (memP.indexOf(Database.medicineDB[pmlist.SelectedIndex]) < 0)
            {
                pplist.Items.Add(string.Format("{0:F}g\t", numpmWeight.Value) + pmlist.Items[pmlist.SelectedIndex]);
            }
            memP.add(Database.medicineDB[pmlist.SelectedIndex],
                decimal.ToDouble(numpmWeight.Value), 0.0);
            memP.sort();
            updatePplist();
            drawPrescriptValue();
        }
        private void updatePplist()
        {
            pplist.Items.Clear();
            foreach (PrescriptNode x in memP.getPrescriptMedicines())
            {
                pplist.Items.Add(string.Format("{0:F}g\t", x.weight) + x.med.name);
            }
        }

        private void txtpmSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = 0;
                foreach (object x in pmlist.Items)
                {
                    if (txtpmSearch.Text.Equals(x)) break;
                    i++;
                }
                if (i < pmlist.Items.Count)
                {
                    txtpmSearch.Text = "";
                    pmlist.SelectedIndex = i;
                }
                else
                {
                    txtpmSearch.SelectionStart = 0;
                    txtpmSearch.SelectionLength = txtpmSearch.Text.Length;
                }
            }
        }

        private void txtpSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = 0;
                foreach (object x in plist.Items)
                {
                    if (txtpSearch.Text.Equals(x)) break;
                    i++;
                }
                if (i < plist.Items.Count)
                {
                    txtpSearch.Text = "";
                    plist.SelectedIndex = i;
                }
                else
                {
                    txtpSearch.SelectionStart = 0;
                    txtpSearch.SelectionLength = txtpSearch.Text.Length;
                }
            }
        }

        private void txtmSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = 0;
                foreach (object x in mlist.Items)
                {
                    if (txtmSearch.Text.Equals(x)) break;
                    i++;
                }
                if (i < mlist.Items.Count)
                {
                    txtmSearch.Text = "";
                    mlist.SelectedIndex = i;
                }
                else
                {
                    txtmSearch.SelectionStart = 0;
                    txtmSearch.SelectionLength = txtmSearch.Text.Length;
                }
            }
        }
    }
}