using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public static class HashHelper
    {
        internal const uint OFFSETBASIS = 2166136261;
        internal const int PRIME = 16777619;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FNVHashCode<T>(params T[] array)
        {
            array.NullCheck();
            unchecked
            {
                int h = (int)HashHelper.OFFSETBASIS;
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != null)
                    {
                        h = (h * HashHelper.PRIME) ^ array[i].GetHashCode();
                    }
                }
                return h;
            }
        }
    }
}
