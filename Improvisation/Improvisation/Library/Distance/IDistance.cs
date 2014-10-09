using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library.Distance
{
    public interface IDistance<T>
    {
        double Distance(T[] first, T[] second);
    }
}
