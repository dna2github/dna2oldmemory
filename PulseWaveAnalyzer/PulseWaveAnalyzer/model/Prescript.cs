using System;
using System.Collections.Generic;
using System.Text;

namespace PulseWaveAnalyzer.model
{
    public class Prescript : IComparable
    {
        public const string version = "J.Y.Liu 2012";
        public int id;
        public string name;
        public double extrascore; public bool extra = false;
        private List<PrescriptNode> meds;

        public Prescript()
        {
            meds = new List<PrescriptNode>();
        }

        public List<PrescriptNode> getPrescriptMedicines()
        {
            return meds;
        }
        public void sort()
        {
            meds.Sort();
        }

        public int indexOf(Medicine m)
        {
            if (m == null) return -1;
            int a, b, mid;
            a = 0; b = meds.Count;
            if (b == 0) return -1;
            while (a <= b)
            {
                mid = (a + b) / 2;
                if (mid < 0 || mid >= meds.Count) return -1;
                if (m.id == meds[mid].med.id) return mid;
                if (m.id < meds[mid].med.id)
                    b = mid - 1;
                else
                    a = mid + 1;
            }
            return -1;
        }
        public void add(Medicine m, double weight, double state)
        {
            if (m == null) return;
            int index = indexOf(m);
            if (index < 0)
                meds.Add(new PrescriptNode(m, weight, 0.0));
            else
                meds[index].weight += weight;
        }
        public void del(Medicine m)
        {
            if (m == null) return;
            int index = indexOf(m);
            if (index < 0) return;
            meds.RemoveAt(index);
        }
        public void clear()
        {
            meds.Clear();
        }

        public double[] prescriptE()
        {
            double[] e = new double[Const.CALC_MERDIAN_NUM];
            for (int i = 0; i < e.Length; i++) e[i] = 0.0;
            foreach (PrescriptNode x in meds)
                for (int i = 0; i < e.Length; i++) e[i] += x.med.merdian[i] * x.weight * x.med.mild;

            double absmax = Double.MinValue;
            for (int i = 0; i < e.Length; i++) if (Math.Abs(e[i]) > absmax) absmax = Math.Abs(e[i]);
            if (absmax == 0.0) return e;
            for (int i = 0; i < e.Length; i++) e[i] /= absmax;
            return e;
        }

        public string toPrescriptString()
        {
            string tmp = "";
            foreach (PrescriptNode x in meds)
            {
                tmp += x.med.id + "," + x.weight + "," + x.state + ";";
            }
            return tmp;
        }

        public int CompareTo(object obj)
        {
            if (obj is Prescript)
            {
                Prescript p = (Prescript)obj;
                if (extra)
                {
                    if (extrascore == p.extrascore) return 0;
                    if (extrascore > p.extrascore) return 1;
                    return -1;
                }
                return id - p.id;
            }
            return 0;
        }

    }
}
