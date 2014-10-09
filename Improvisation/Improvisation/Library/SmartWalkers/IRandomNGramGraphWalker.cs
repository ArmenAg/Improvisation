using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public interface INGramRandomGraphWalker<T>
        where T : IEquatable<T>
    {
        void LoadGraph(NGramGraphMarkovChain<T> graph);

        NGram<T> Next(NGram<T>[] chain);

        IEnumerable<NGram<T>> NextMultiple(NGram<T>[] chain, int depth);
    }
}
