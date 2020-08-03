using System;
using System.Collections.Generic;
using System.Text;

namespace PulseWaveAnalyzer.model
{
    public class Medicine : IComparable
    {
        public const string version = "J.Y.Liu 2012";
        public int id;
        public string name;
        public double mild;
        public double[] merdian; // HT,LR,KI,SP,LU,ST,GB,BL,LI,TB,SI,PC

        public Medicine()
        {
            id = 0;
            name = null;
            mild = 0.0;
            merdian = new double[Const.CALC_MERDIAN_NUM];
            for (int i = 0; i < merdian.Length; i++) merdian[i] = 0.0;
        }

        public int CompareTo(object obj)
        {
            if (obj is Medicine)
            {
                return id - ((Medicine)obj).id;
            }
            return 0;
        }
    }
}
