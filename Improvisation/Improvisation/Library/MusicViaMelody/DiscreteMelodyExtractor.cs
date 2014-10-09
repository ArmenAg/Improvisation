using Improvisation.Library.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.MusicViaMelody
{
    public static class DiscreteMelodyExtractor
    {
        public static IOrderedEnumerable<CycleData<NGram<Chord>>> ExtractMelody(NGram<Chord>[] a)
        {
            a.NullCheck();
            return CycleFinder.BruteForceAlgorithm(a.ToList())
                            .Where(x => x.NumberOfCycles != 0)
                            .OrderByDescending(x => x.Amplitude);
        }
    }
}
