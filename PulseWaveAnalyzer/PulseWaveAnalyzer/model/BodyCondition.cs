using System;
using System.Collections.Generic;
using System.Text;

namespace PulseWaveAnalyzer.model
{
    public class BodyCondition
    {
        public const string version = "J.Y.Liu 2012";
        public static double[] bodyS = new double[] {
            100.000000, 80.388410, 53.670830, 41.030000, 22.520010, 17.64398,
            12.482200, 6.949338, 3.963352, 2.626356, 1.791302, 1.183306,
            /* extra */ 0.811512
        };
        public double[] bodyM;

        public BodyCondition()
        {
            bodyM = new double[Const.CALC_MERDIAN_NUM];
        }

        public double[] bodyconditionE()
        {
            double[] e = new double[Const.CALC_MERDIAN_NUM];

            e[0] = 0;
            for (int i = 1; i < e.Length; i++) e[i] = (bodyM[i] - bodyS[i]) / bodyS[i];
            return e;
        }

        public double[] eatmedicineE(double[] mE)
        {
            double[] e = new double[mE.Length];

            double eHT = 1 + mE[0] * Const.MEDICINE_EFFECTIVITY;
            e[0] = 0;
            for (int i = 1; i < e.Length; i++)
                e[i] = (bodyM[i] * (1 + mE[i] * Const.MEDICINE_EFFECTIVITY) / eHT - bodyS[i]) / bodyS[i];
            return e;
        }

        public double scoreBodyCondition(double[] ef)
        {
            double rms = 0.0;
            for (int i = 0; i < ef.Length; i++) rms += ef[i] * ef[i];
            return Math.Sqrt(rms);
        }
    }
}
