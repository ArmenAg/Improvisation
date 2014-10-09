using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AForge.Math
{
    public static class GammaFunction
    {
        public static double LogGammaLanczos(double x)
        {
            double[] coef = new double[6] { 76.18009172947146,
    -86.50532032941677, 24.01409824083091,
    -1.231739572450155, 0.1208650973866179E-2,
    -0.5395239384953E-5 };

            double LogSqrtTwoPi = 0.91893853320467274178;
            double denom = x + 1;
            double y = x + 5.5;
            double series = 1.000000000190015;
            for (int i = 0; i < 6; ++i)
            {
                series += coef[i] / denom;
                denom += 1.0;
            }
            return (LogSqrtTwoPi + (x + 0.5) * System.Math.Log(y) -
            y + System.Math.Log(series / x));
        }

        public static double LogGammaContinued(double x)
        {
            const double a0 = 1.0D / 12D;
            const double a1 = 1.0D / 30D;
            const double a2 = 53.0D / 210D;
            const double a3 = 195.0D / 371D;
            const double a4 = 22999.0D / 22737D;
            const double a5 = 29944523.0D / 19733142D;
            const double a6 = 109535241009.0D / 48264275462D;

            double t6 = a6 / x;
            double t5 = a5 / (x + t6);
            double t4 = a4 / (x + t5);
            double t3 = a3 / (x + t4);
            double t2 = a2 / (x + t3);
            double t1 = a1 / (x + t2);
            double t0 = a0 / (x + t1);

            double result = t0 - x + ((x - 0.5) *
              System.Math.Log(x)) +
              (0.5 * System.Math.Log(2 * System.Math.PI));
            return result;
        }
    }
}
