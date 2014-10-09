using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Improvisation.Library;

namespace Improvisation.FinalUI
{
    internal static class FinalUIHelperMethods
    {
        public static string FileFriendlyString(string s)
        {
            s.NullCheck();
            return s.Split('\\').Last();
        }
    }
}
