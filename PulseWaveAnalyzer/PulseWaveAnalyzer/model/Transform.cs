using System;
using System.Collections.Generic;
using System.Text;

namespace PulseWaveAnalyzer.model
{
    class Transform
    {
        public const string version = "J.Y.Liu 2012";
        public const double dipi = 6.2831853071795864769252867665590057683943;

        public static double[] real_fft(int K, double[] realx)
        {
            return real_fft(K, realx.Length, realx);
        }
        public static double[] real_fft(int K, int N, double[] realx)
        {
            double[] cos, sin;
            cos = new double[N];
            sin = new double[N];
            for (int i = 0; i < N; i++)
            {
                cos[i] = Math.Cos(2 * Math.PI * i / N);
                sin[i] = Math.Sin(2 * Math.PI * i / N);
            }

            double[] re, im;
            int idec, tmpr;
            re = new double[K];
            im = new double[K];
            for (int i = 1; i <= K; i++)
            {
                idec = i - 1;
                re[idec] = 0.0; im[idec] = 0.0;
                for (int j = 0; j < N; j++)
                {
                    tmpr = (i * j) % N;
                    re[idec] += realx[j] * cos[tmpr];
                    im[idec] += realx[j] * sin[tmpr];
                }
            }

            for (int i = 0; i < K; i++)
            {
                re[i] = Math.Sqrt(re[i] * re[i] + im[i] * im[i]);
            }
            return re;
        }

        ///* FFT */
        //public static Complex[] fft(int N, Complex[] x)
        //{
        //    /* Declare a pointer to scratch space. */
        //    Complex[] XX = new Complex[2 * N];
        //    for (int i = 0; i < 2 * N; i++)
        //    {
        //        XX[i] = new Complex();
        //    }

        //    Complex[] X = new Complex[N];
        //    for (int i = 0; i < N; i++)
        //    {
        //        X[i] = new Complex();
        //    }

        //    /* Calculate FFT by a recursion. */
        //    fft_rec(N, 0, 1, x, X, XX);
        //    return X;
        //}

        ///* FFT recursion */
        //public static void fft_rec(int N, int offset, int delta,
        //             Complex[] x, Complex[] X, Complex[] XX)
        //{
        //    int N2 = N / 2;            /* half the number of points in FFT */
        //    int k;                   /* generic index */
        //    double cs, sn;           /* cosine and sine */
        //    int k00, k01, k10, k11;  /* indices for butterflies */
        //    double tmp0, tmp1;       /* temporary storage */

        //    if (N != 2)  /* Perform recursive step. */
        //    {
        //        /* Calculate two (N/2)-point DFT's. */
        //        fft_rec(N2, offset, 2 * delta, x, XX, X);
        //        fft_rec(N2, offset + delta, 2 * delta, x, XX, X);

        //        /* Combine the two (N/2)-point DFT's into one N-point DFT. */
        //        for (k = 0; k < N2; k++)
        //        {
        //            k00 = offset + k * delta; k01 = k00 + N2 * delta;
        //            k10 = offset + 2 * k * delta; k11 = k10 + delta;
        //            cs = Math.Cos(dipi * k / (double)N); sn = Math.Sin(dipi * k / (double)N);
        //            tmp0 = cs * XX[k11].x + sn * XX[k11].y;
        //            tmp1 = cs * XX[k11].y - sn * XX[k11].x;
        //            X[k01].x = XX[k10].x - tmp0;
        //            X[k01].y = XX[k10].y - tmp1;
        //            X[k00].x = XX[k10].x + tmp0;
        //            X[k00].y = XX[k10].y + tmp1;
        //        }
        //    }
        //    else  /* Perform 2-point DFT. */
        //    {
        //        k00 = offset; k01 = k00 + delta;
        //        X[k01].x = x[k00].x - x[k01].x;
        //        X[k01].y = x[k00].y - x[k01].y;
        //        X[k00].x = x[k00].x + x[k01].x;
        //        X[k00].y = x[k00].y + x[k01].y;
        //    }
        //}

        ///* IFFT */
        //public static Complex[] ifft(int N, Complex[] X)
        //{
        //    int N2 = N / 2;       /* half the number of points in IFFT */
        //    int i;              /* generic index */
        //    double tmp0, tmp1;  /* temporary storage */

        //    /* Calculate IFFT via reciprocity property of DFT. */
        //    Complex[] x = fft(N, X);
        //    x[0].x = x[0].x / N; x[0].y = x[0].y / N;
        //    x[N2].x = x[N2].x / N; x[N2].y = x[N2].y / N;
        //    for (i = 1; i < N2; i++)
        //    {
        //        tmp0 = x[i].x / N; tmp1 = x[i].y / N;
        //        x[i].x = x[N - i].x / N; x[i].y = x[N - i].y / N;
        //        x[N - i].x = tmp0; x[N - i].y = tmp1;
        //    }
        //    return x;
        //}
    }
}
