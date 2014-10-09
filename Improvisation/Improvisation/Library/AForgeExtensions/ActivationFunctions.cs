using AForge.Neuro;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AForge.Neuro
{
    [Serializable]
    public class FractionalSigmoidFunction : IActivationFunction, ISerializable
    {
        public double Alpha
        {
            get
            {
                return this.innerFunction.Alpha;
            }
            set
            {
                this.innerFunction.Alpha = value;
            }
        }

        public readonly double DistrubutionAverage = 1.0D;
        public readonly double DistrubutionDeviation = 0.2D;

        [NonSerialized]
        private readonly SigmoidFunction innerFunction;
        [NonSerialized]
        private readonly Normal normal;

        public FractionalSigmoidFunction(SerializationInfo info, StreamingContext context)
            : this() { }
        public FractionalSigmoidFunction()
        {
            this.innerFunction = new SigmoidFunction();
            this.normal = new Normal(this.DistrubutionAverage, this.DistrubutionDeviation);
        }
        public double Derivative(double x)
        {
            return this.Derivative(x, this.normal.Sample());
        }

        public double Derivative2(double y)
        {
            return this.Derivative(y);
        }
        private double Derivative(double x, double derivativeStep)
        {
            double exp = System.Math.Exp(x);
            double top = exp * System.Math.Pow(x, derivativeStep);
            double lower = derivativeStep * exp + derivativeStep;
            return Accord.Math.Gamma.Function(derivativeStep + 1) * (top / lower);
        }
        public double Function(double x)
        {
            return this.innerFunction.Function(x);
        }

        public static void Test()
        {
            FractionalSigmoidFunction frac = new FractionalSigmoidFunction();
            SigmoidFunction norm = new SigmoidFunction();

            double left = frac.Derivative(0.5, 1);
            double right = norm.Derivative(0.5);

            Debugger.Break();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("innerFunction", new SigmoidFunction(), typeof(SigmoidFunction));
            info.AddValue("normal", new Normal(1.15D, 0.2D), typeof(Normal));
        }
    }
    [Serializable]
    public class SoftPlusFunction : IActivationFunction
    {
        public double Derivative(double x)
        {
            return 1D / (1D + System.Math.Exp(-x));
        }

        public double Derivative2(double y)
        {
            return this.Derivative(y);
        }

        public double Function(double x)
        {
            return System.Math.Log(1 + System.Math.Exp(x));
        }
    }
    [Serializable]
    public class RectifierSoftPlusFunction : IActivationFunction
    {
        public double Derivative(double x)
        {
            return 1D / (1D + System.Math.Exp(-x));
        }

        public double Derivative2(double y)
        {
            return this.Derivative(y);
        }

        public double Function(double x)
        {
            return System.Math.Max(0, x);
        }
    }
    public class NoisyRectifierSoftPlusFunction : IActivationFunction
    {
        public double Derivative(double x)
        {
            return 1D / (1D + System.Math.Exp(-x + RectiferNormalHelper.RectifierNormal()));
        }

        public double Derivative2(double y)
        {
            return this.Derivative(y);
        }

        public double Function(double x)
        {
            return System.Math.Max(0, x + RectiferNormalHelper.RectifierNormal());
        }
    }
    static class RectiferNormalHelper
    {
        public static double Std { get; set; }
        static RectiferNormalHelper()
        {
            Std = 0.2;
        }
        public static double RectifierNormal()
        {
            Normal m = new Normal(0, Std);
            return m.Sample();
        }
    }
}
