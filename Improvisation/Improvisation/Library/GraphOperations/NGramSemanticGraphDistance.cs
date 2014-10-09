using Improvisation.Library.Distance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.GraphOperations
{
    public class NGramSemanticGraphDistance<T>
        where T : IEquatable<T>
    {
        public float TolerancePerNode { get; set; }
        public float EmptyNodeWeight { get; set; }
        public IDistance<float> ProbabilityDistace { get; set; }

        public readonly Func<float, float> TransformFunction;

        public NGramSemanticGraphDistance()
            : this(new EuclideanDistance<float>(), new Func<float, float>(x => x)) { }
        public NGramSemanticGraphDistance(IDistance<float> e, Func<float, float> transform, float tolerance = 0F, float empty = 1F)
        {
            e.NullCheck();
            transform.NullCheck();

            this.TolerancePerNode = tolerance;
            this.EmptyNodeWeight = empty;

            this.ProbabilityDistace = e;
            this.TransformFunction = transform;
        }

        public float Distance(NGramGraphMarkovChain<T> left, NGramGraphMarkovChain<T> right, NGramGraphDistanceType type = NGramGraphDistanceType.CompleteGraph)
        {
            left.NullCheck();
            right.NullCheck();

            switch (type)
            {
                case NGramGraphDistanceType.CompleteGraph:
                    return (this.DistanceCompleteWithRespectToLeft(left, right) + this.DistanceCompleteWithRespectToLeft(right, left)) / 2F;
                case NGramGraphDistanceType.SubGraph:
                    if (left.Count() > right.Count())
                    {
                        return this.DistanceCompleteWithRespectToLeft(right, left);
                    }
                    return this.DistanceCompleteWithRespectToLeft(left, right);
            }
            throw new NotImplementedException();
        }
        private float DistanceCompleteWithRespectToLeft(NGramGraphMarkovChain<T> left, NGramGraphMarkovChain<T> right)
        {
            float sum = 0;
            foreach (var item in left)
            {
                var node = item.Key;
                if (!right.ValidNode(node))
                {
                    sum += this.ToleranceTransform(this.TransformFunction(this.EmptyNodeWeight));
                    continue;
                }
                var rightConnections = right.Edges(node).Select(y => this.TransformFunction(y.Probability)).ToArray();
                var total = this.TransformFunction((float)this.ProbabilityDistace.Distance(item.Value.Select(x => this.TransformFunction(x.Probability)).ToArray(), rightConnections));
                if (total / (float)left.Count() < this.TolerancePerNode)
                {
                    sum += 0;
                }
                else
                {
                    sum += total;
                }
            }
            return sum / (this.TransformFunction(1F) * (float)right.Grams.Count());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float ToleranceTransform(float f)
        {
            if (f < this.TolerancePerNode)
            {
                return 0;
            }
            return f;
        }
    }
}
