using AForge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public static class CycleFinder
    {
        public static HashSet<IntRange> Cycles<T>(List<T> list)
            where T : IEquatable<T>
        {
            list.NullCheck();


        }
    }
}
