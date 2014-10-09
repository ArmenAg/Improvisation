using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.SmartWalkers
{

    public interface INGramWeightAssigner<T>
        where T : IEquatable<T>
    {
        Dictionary<NGram<T>, float> NextPossibleStateAssignment(NGram<T>[] past, NGramGraphMarkovChain<T> subGraph);
    }
}
