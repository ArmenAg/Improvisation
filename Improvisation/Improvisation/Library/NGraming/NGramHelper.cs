using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improvisation.Library
{
    public static class NGramHelper
    {
        public static string ShowNGram<T>(IEnumerable<NGram<T>> gram)
            where T : IEquatable<T>
        {
            if (typeof(T) == typeof(string))
            {
                return NGramHelper.BuildStringForTypeString((IEnumerable<NGram<string>>)gram);
            }
            return BuildStringGeneric<T>(gram);
        }
        private static string BuildStringGeneric<T>(IEnumerable<NGram<T>> a)
            where T : IEquatable<T>
        {
            var list = a.ToList();
            StringBuilder builder = new StringBuilder(list.Count * list.First().N * 6);

            foreach (var gram in list)
            {
                foreach (var item in gram)
                {
                    builder.Append(item.ToString());
                    builder.Append(" ");
                }
            }
            if (builder.Length > 1)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();

        }
        private static string BuildStringForTypeString(IEnumerable<NGram<string>> a)
        {
            var list = a.ToList();
            StringBuilder builder = new StringBuilder(list.Count * list.First().N * 6);

            foreach (var gram in list)
            {
                foreach (var item in gram)
                {
                    builder.Append(item);
                    builder.Append(" ");
                }
            }
            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
    }
}
