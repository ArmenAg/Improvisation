using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Improvisation.Library.Data
{
    internal static class TextCorpus
    {
        private const string FolderReference = "CorpusData";

        public static List<string> RetrieveAnyCompleteWordCorpus(bool normalize = true)
        {
            string filePath = Directory.EnumerateFiles(TextCorpus.FolderReference).First();
            return XmlAffectiveTextReader
                .Read(filePath)
                .Select(word =>
                    {
                        if (normalize)
                        {
                            return new string(word.Where(x => char.IsLetterOrDigit(x)).ToArray()).ToLower();
                        }
                        return word;
                    })
                .ToList();
        }
        public static List<char> RetrieveAnyCharacterCorpus(bool normalize = true)
        {
            var list = TextCorpus.RetrieveAnyCompleteWordCorpus(normalize);
            List<char> characters = new List<char>(list.Count * 8);
            foreach (var item in list)
            {
                characters.AddRange(item);
            }
            return characters;
        }
        /// <summary>
        /// Affective text is the name of the database
        /// </summary>
        private static class XmlAffectiveTextReader
        {
            public static List<string> Read(string path)
            {
                XDocument doc = XDocument.Load(File.OpenRead(path));
                XElement root = doc.Root;

                var list = root
                    .Elements()
                    .Select(child => child.Value.Split(' '))
                    .ToList();

                List<string> a = new List<string>(list.Count * 6);
                foreach (var item in list)
                {
                    a.AddRange(item);
                }
                return a;
            }
        }
    }
}
