using System;
using System.Collections.Generic;
using System.Text;

using PulseWaveAnalyzer.model;
using System.Diagnostics;

namespace Tester
{
    class Program
    {
        static double[][] bodym;
        static void Main(string[] args)
        {
            List<Medicine> ms = new List<Medicine>();
            List<Prescript> ps = new List<Prescript>();

            //Medicine m = new Medicine();
            //Prescript p = new Prescript();

            //m.name = "test";
            //m.mild = 0.0;
            //m.merdian[1] = 1.0;
            //m.merdian[3] = 1.0;

            //p.name = "testtoo";
            //p.add(m, 1.0, 0.0);

            //DBParser.addMedicine(ms, m);
            //DBParser.addPrescript(ps, p);
            //DBParser.removeMedicine(1);
            //DBParser.removePrescript(1);
            ms = DBParser.medicineDB();
            ps = DBParser.prescriptDB(ms);

            bodym = new double[][] {
                //new double[] {100.00,80.38,53.67,41.03,22.52,17.64,12.48,6.94,3.96,2.63,1.79,1.18},
                new double[] {100.00,37.06,43.74,17.35,9.22,5.96,3.98,2.03,1.66,1.64,2.04,1.18},
                new double[] {100.00,93.06,55.16,17.98,9.59,6.28,2.20,1.54,1.25,0.79,0.43,1.18},
                new double[] {100.00,41.10,40.97,17.23,7.18,5.27,2.68,2.82,1.29,1.58,1.00,1.18},
                new double[] {100.00,50.06,50.85,33.10,14.91,13.26,12.50,8.35,4.48,2.54,2.33,1.18},
                new double[] {100.00,41.14,28.95,41.27,20.56,11.40,15.90,9.59,6.59,3.33,3.95,1.18}
            };
            int testid = 3;

            double[] ef,efm,e;
            double s, sm;
            List<Prescript> result = new List<Prescript>();
            BodyCondition body = new BodyCondition();

            body.bodyM = bodym[testid];
            ef = body.bodyconditionE();
            s = body.scoreBodyCondition(ef);
            foreach (Prescript x in ps)
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

            foreach (Prescript x in result)
            {
                Debug.WriteLine(string.Format(x.name + "  [E]{0:F}",
                    ((s - x.extrascore) / s * 100)));
            }
        }
    }
}
