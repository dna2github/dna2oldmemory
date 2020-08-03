using System;
using System.Collections.Generic;
using System.Text;

namespace PulseWaveAnalyzer.model
{
    public class PrescriptNode : IComparable
    {
        public const string version = "J.Y.Liu 2012";
        public Medicine med;
        public double weight;
        public double state; // reserved

        public PrescriptNode(Medicine m, double w, double s)
        {
            med = m;
            weight = w;
            state = s;
        }

        public int CompareTo(object obj)
        {
            if (obj is PrescriptNode)
            {
                PrescriptNode x = (PrescriptNode)obj;
                if (x.med == null) return 1;
                if (med == null) return -1;
                return med.id - x.med.id;
            }
            return 0;
        }
    }
}
