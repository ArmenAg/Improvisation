using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Distance
{
    public sealed class GaussianNoiseDistance<T> : IDistance<T>
    {
        public readonly IDistance<T> InnerDistance;

        public readonly float StandardDeviation;
        public readonly float Mean = 1;

        private readonly Normal normal;

        public GaussianNoiseDistance(IDistance<T> distance, float std)
        {
            distance.NullCheck();
            (std >= 0).AssertTrue();

            this.InnerDistance = distance;
            this.StandardDeviation = std;

            this.normal = new Normal(this.Mean, this.StandardDeviation);
        }

        public double Distance(T[] first, T[] second)
        {
            return this.InnerDistance.Distance(first, second) * this.normal.Sample();
        }
    }
}
