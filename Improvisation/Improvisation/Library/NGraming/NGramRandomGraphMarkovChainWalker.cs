using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public class NGramRandomHomogenousMatrixMarkovChainWalker<T>
        where T : IEquatable<T>
    {
        public readonly Random Random;

        private readonly HomogenousNGramMatrixMarkovChain<T> chain;
        private readonly Vector<float> stateZero;

        public NGramRandomHomogenousMatrixMarkovChainWalker(HomogenousNGramMatrixMarkovChain<T> chain, Vector<float> stateZero, Random rand)
        {
            chain.NullCheck();
            stateZero.NullCheck();
            rand.NullCheck();

            chain.ValidState(stateZero).AssertTrue();

            this.chain = chain;
            this.stateZero = stateZero;

            this.Random = rand;
        }

        public NGram<T>[] WalkMarkovChain(int depth)
        {
            List<NGram<T>> path = new List<NGram<T>>(depth);

            (depth > 0).AssertTrue();

            var state = this.stateZero;
            path.Add(this.RetrieveForSingleState(state));

            for (int i = 1; i < depth; i++)
            {
                state = this.chain.RetrieveNextState(state);
                var node = this.RetrieveForSingleState(state);
                if (null == node)
                {
                    return path.ToArray();
                }
                path.Add(node);
                state = this.chain.RetrieveStateZero(new HashSet<NGram<T>>(node.AsEnumerableObject()));
            }
            return path.ToArray();
        }
        private NGram<T> RetrieveForSingleState(Vector<float> curState)
        {

            while (true)
            {
                if (curState.Count == 0)
                {
                    return NGram<T>.Empty;
                }
                for (int i = 0; i < curState.Count; i++)
                {
                    if (curState[i] != 0)
                    {
                        if (this.Random.NextDouble() < curState[i])
                        {
                            return this.chain.RetrieveNodeAtIndex(i);
                        }
                    }
                }
            }
        }
    }
    public class NGramRandomGraphMarkovChainWalker<T>
        where T : IEquatable<T>
    {
        public enum NextStateType : byte
        {
            PossibleHeterogenous = 0,
            Homogenous = 1
        }
        public readonly Random Random;

        private readonly NGramGraphMarkovChain<T> graph;
        public readonly NGram<T> startingNode;
        public NGramRandomGraphMarkovChainWalker(NGramGraphMarkovChain<T> graph, NGram<T> startingNode, Random rand)
        {
            graph.NullCheck();
            graph.ValidNode(startingNode).AssertTrue();

            rand.NullCheck();

            this.graph = graph;
            this.startingNode = startingNode;
            this.Random = rand;
        }

        public NGram<T>[] WalkMarkovChain(int depth, NextStateType searchType = NextStateType.PossibleHeterogenous)
        {
            List<NGram<T>> path = new List<NGram<T>>(depth);
            (depth > 0).AssertTrue();

            NGram<T> curState = this.startingNode;
            path.Add(curState);
            for (int i = 1; i < depth; i++)
            {
                curState = this.GetNextState(curState, searchType);
                if (curState == NGram<T>.Empty)
                {
                    return path.ToArray();
                }
                path.Add(curState);
            }
            return path.ToArray();
        }

        private NGram<T> GetNextState(NGram<T> curState, NextStateType searchType)
        {
            var edges = this.graph.Edges(curState);
            if (null == edges || edges.Count == 0)
            {
                switch (searchType)
                {
                    case NextStateType.PossibleHeterogenous:
                        var pos = this.graph.GetLowerConstruct(curState);
                        if (pos.HasValue)
                        {
                            curState = pos.Value;
                        }
                        else
                        {
                            return NGram<T>.Empty;
                        }
                        break;
                    case NextStateType.Homogenous:
                        return NGram<T>.Empty;
                }
            }
            while (true)
            {
                foreach (var item in edges)
                {
                    if (this.Random.NextDouble() < item.Probability)
                    {
                        return item.Edge;
                    }
                }
            }

        }
    }

}
