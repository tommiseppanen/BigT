using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BigT
{
    public static class Big
    {
        public static string T(string text)
        {
            return "'" + text + "'";
        }

        public static void LoadTranslations(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            {
                var data = Csv.ParseHeadAndTail(reader, ',', '"');
                var header = data.Item1;
                var lines = data.Item2;

                var values = new Dictionary<string, Dictionary<string, string>>();
                foreach (var line in lines)
                {
                    if (values.ContainsKey(line[0]))
                        continue;

                    var translations = new Dictionary<string, string>();
                    values.Add(line[0], translations);
                }
            }
        }
    }
}
