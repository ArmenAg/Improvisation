using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Improvisation.Library.Music
{
    public interface IDynamicHash
    {
        Func<int> DynamicHashFunction { get; set; }
        Predicate<IDynamicHash> DynamicEqualFunction { get; set; }
    }
}
