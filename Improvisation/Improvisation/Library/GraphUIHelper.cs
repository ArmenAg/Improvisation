using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public static class GraphUIHelper
    {
        public static GraphExample GenerateGraphUI<T>(NGramGraphMarkovChain<T> markovChain)
                    where T : IEquatable<T>
        {
            var dataGraph = new GraphExample();

            Dictionary<NGram<T>, int> indices = new Dictionary<NGram<T>, int>();
            int i = 0;
            foreach (var item in markovChain)
            {
                var dataVertex = new DataVertex(NGramHelper.ShowNGram<T>(item.Key.AsEnumerableObject())) { ID = i };
                dataGraph.AddVertex(dataVertex);
                indices.Add(item.Key, i);
                i++;
            }
            var vlist = dataGraph.Vertices.ToList();

            foreach (var item in markovChain)
            {
                foreach (var edge in item.Value)
                {
                    var dataEdge = new DataEdge(vlist[indices[item.Key]], vlist[indices[edge.Edge]], edge.Probability) { Text = edge.Probability.ToString() };
                    dataGraph.AddEdge(dataEdge);
                }
            }

            return dataGraph;
        }
    }
}
