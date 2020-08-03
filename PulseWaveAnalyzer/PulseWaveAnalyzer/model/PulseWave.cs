using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PulseWaveAnalyzer.model
{
    class PulseWave
    {
        public const string version = "J.Y.Liu 2012";
        public const int PULSEWAVE_RAW_PER_SEC = 1830;
        public const int PULSEWAVE_RAW_SEC = 60;

        public double[] raw;
        public double max, min;

        public double[] gr;
        public double grmax, grmin;

        public double[] rawfft;
        public double fftmax, fftmin;

        public void Load(string filename)
        {
            raw = new double[PULSEWAVE_RAW_PER_SEC * PULSEWAVE_RAW_SEC];

            using (FileStream _fs = File.OpenRead(filename))
            {
                _fs.ReadByte(); _fs.ReadByte();
                _fs.ReadByte(); _fs.ReadByte(); // skip data count
                max = double.MinValue;
                min = double.MaxValue;

                byte[] tmp = new byte[PULSEWAVE_RAW_PER_SEC * PULSEWAVE_RAW_SEC * 2];
                _fs.Read(tmp, 0, tmp.Length);
                _fs.Close();
                for (int i = 0; i < PULSEWAVE_RAW_PER_SEC * PULSEWAVE_RAW_SEC; i++)
                {
                    //raw[i] = _fs.ReadByte() + 256.0 * _fs.ReadByte();
                    raw[i] = tmp[i * 2] + 256.0 * tmp[i * 2 + 1];
                    if (raw[i] > max) max = raw[i];
                    if (raw[i] < min) min = raw[i];
                }
            }

            return;
        }

        public void Group(int group)
        {
            gr = null;
            if (raw == null) return;
            if (group <= 0) return;
            gr = new double[group];

            int x = 0, y = 0;
            int xc = 0, yc = 0;
            int nowc, baseindex;
            if (raw.Length % group == 0)
            {
                x = group; y = 0;
                xc = raw.Length / group; yc = 0;
            }
            else
            {
                yc = raw.Length / group;
                xc = yc + 1;
                x = raw.Length % group;
                y = group - x;
            }
            nowc = xc;
            baseindex = 0;

            double aggr = 0.0;
            grmax = double.MinValue;
            grmin = double.MaxValue;

            for (int i = 0; i < group; i++)
            {
                if (i == x)
                {
                    nowc = yc;
                    baseindex = x;
                }
                aggr = 0.0;
                for (int j = 0; j < nowc; j++)
                {
                    aggr += raw[baseindex + nowc * i + j];
                }
                aggr /= nowc;
                gr[i] = aggr;
                if (aggr > grmax) grmax = aggr;
                if (aggr < grmin) grmin = aggr;
            }

            return;
        }

        public double[] getF(double f1, double f2, int N, int samplesPerSec)
        {
            return getFinAvg(f1, f2, N, samplesPerSec);
        }
        public double[] getFinAvg(double f1, double f2, int N, int samplesPerSec)
        {
            if (rawfft == null) return null;

            double[] r = new double[Const.CALC_MERDIAN_NUM];
            int k1, k2;
            k1 = (int)(f1 * N / samplesPerSec);
            k2 = (int)(f2 * N / samplesPerSec) + 1;

            if (k1 < 0) k1 = 0;
            if (k1 >= rawfft.Length) k1 = rawfft.Length - 1;
            if (k2 >= rawfft.Length) k2 = rawfft.Length - 1;

            int k0; double f0;
            f0 = -1.0; k0 = 0;
            for (int i = k1; i <= k2; i++)
            {
                if (rawfft[i] > f0) { k0 = i; f0 = rawfft[i]; }
            }
            k1 = k0 - k1; k2 = k2 - k0;
            k1 = 3; k2 = 3; // modified

            if (f0 == 0.0) f0 = 1.0;
            double avg = 0.0;
            for (int i = 1; i <= Const.CALC_MERDIAN_NUM; i++)
            {
                // search for a range to get a local average
                avg = 0.0;
                for (int j = k0 * i - k1; j <= k0 * i + k2; j++)
                {
                    avg += rawfft[j];
                }
                avg /= k2 + k1 + 1;
                r[i - 1] = avg;
            }
            for (int i = Const.CALC_MERDIAN_NUM - 1; i >= 0; i--)
            {
                r[i] = r[i] / r[0] * 100;
            }
            return r;
        }
        public double[] getFinMax(double f1, double f2, int N, int samplesPerSec)
        {
            if (rawfft == null) return null;

            double[] r = new double[Const.CALC_MERDIAN_NUM];
            int k1, k2;
            k1 = (int)(f1 * N / samplesPerSec);
            k2 = (int)(f2 * N / samplesPerSec) + 1;
            
            if (k1 < 0) k1 = 0;
            if (k1 >= rawfft.Length) k1 = rawfft.Length - 1;
            if (k2 >= rawfft.Length) k2 = rawfft.Length - 1;

            int k0; double f0;
            f0 = -1.0; k0 = 0;
            for (int i = k1; i <= k2; i++)
            {
                if (rawfft[i] > f0) { k0 = i; f0 = rawfft[i]; }
            }
            k1 = k0 - k1; k2 = k2 - k0;

            if (f0 == 0.0) f0 = 1.0;
            for (int i = 1; i <= Const.CALC_MERDIAN_NUM; i++)
            {
                if (k0 * i >= rawfft.Length) break;
                // search for a range to get a local maximum
                for (int j = k0 * i - k1; j <= k0 * i + k2; j++)
                {
                    if (rawfft[j] > r[i - 1]) r[i - 1] = rawfft[j];
                }
            }
            for (int i = 0; i < Const.CALC_MERDIAN_NUM; i++)
            {
                r[i] = r[i] / f0 * 100;
            }
            return r;
        }

        public void FourierTransform()
        {
            if (raw == null) return;

            int tovalue = 1024;
            int N = 50000;
            rawfft = Transform.real_fft(tovalue, N, raw); 

            //rawfft[0] = raw[0];
            //fftmax = rawfft[0]; fftmin = rawfft[0];
            fftmax = 0; fftmin = 0;
            for (int k = 0; k < tovalue; ++k)
            {
                if (rawfft[k] > fftmax) fftmax = rawfft[k];
                if (rawfft[k] < fftmin) fftmin = rawfft[k];
            }
        }
    }
}
