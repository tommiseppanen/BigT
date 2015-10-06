using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BigT
{
    public static class Big
    {
        private const string defaultLanguageIdentifier = "Default";

        private static List<string> languages = new List<string>() { defaultLanguageIdentifier };
        private static Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>();
        private static string activeLanguage = defaultLanguageIdentifier;

        public static string T(string text)
        {
            return T(text, activeLanguage);
        }

        public static string T(string text, string language)
        {
            if (translations.ContainsKey(text) && translations[text].ContainsKey(activeLanguage))
                return translations[text][activeLanguage];

            return text;
        }

        public static void SetLanguage(string language)
        {
            activeLanguage = language;
        }

        public static void LoadTranslations(string path)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            {
                var data = Csv.ParseHeadAndTail(reader, ',', '"');
                languages = data.Item1.ToList();
                translations = TranslationsToDictionary(data.Item2, languages);   
            }
        }

        private static Dictionary<string, Dictionary<string, string>> TranslationsToDictionary(IEnumerable<IList<string>> lines,
            List<string> languageList)
        {
            var translationDictionary = new Dictionary<string, Dictionary<string, string>>();
            foreach (var line in lines)
            {
                if (translationDictionary.ContainsKey(line[0]))
                    continue;

                translationDictionary.Add(line[0], LineToLanguageDictionary(line, languageList));
            }

            return translationDictionary;
        }

        private static Dictionary<string, string> LineToLanguageDictionary(IList<string> line, List<string> languageList)
        {
            var values = new Dictionary<string, string>();
            for (int i = 1; i < line.Count; i++)
            {
                values.Add(languageList.ElementAt(i), line[i]);
            }

            return values;
        }
    }
}
