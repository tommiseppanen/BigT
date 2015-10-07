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

                if (data.Item1 != null)
                    languages = data.Item1.ToList();

                translations = MapDefaultValueToTranslations(data.Item2, languages);   
            }
        }

        private static Dictionary<string, Dictionary<string, string>> MapDefaultValueToTranslations(IEnumerable<IList<string>> rows,
            List<string> languageList)
        {
            var translationDictionary = new Dictionary<string, Dictionary<string, string>>();
            foreach (var translations in rows)
            {
                if (translationDictionary.ContainsKey(translations[0]))
                    continue;

                translationDictionary.Add(translations[0], MapLanguagesToTranslations(languageList, translations));
            }

            return translationDictionary;
        }

        private static Dictionary<string, string> MapLanguagesToTranslations(List<string> languageList, IList<string> row)
        {
            var values = new Dictionary<string, string>();
            for (int i = 1; i < row.Count; i++)
            {
                values.Add(languageList.ElementAt(i), row[i]);
            }

            return values;
        }

        public static void SaveTranslations(string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (StreamWriter file = new StreamWriter(path))
            {
                WriteLanguages(file);
                WriteTranslations(file);
            }
        }

        private static void WriteLanguages(StreamWriter file)
        {
            for (int i = 0; i < languages.Count; i++)
            {
                if (i>0)
                    file.Write(",");

                file.Write(ConvertToCsvSafe(languages[i]));
            }
            file.WriteLine();
        }

        private static void WriteTranslations(StreamWriter file)
        {
            foreach (var translatedString in translations)
            {
                file.Write(ConvertToCsvSafe(translatedString.Key));
                foreach (var translationPair in translatedString.Value)
                {
                    file.Write(",");
                    file.Write(ConvertToCsvSafe(translationPair.Value));
                }
                file.WriteLine();
            }
        }

        private static string ConvertToCsvSafe(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                return '"' + value.Replace("\"", "\"\"") + '"';

            return value;
        }

        public static void UpdateTranslations(IEnumerable<string> items)
        {
            var updatedTranslations = new Dictionary<string, Dictionary<string, string>>();
            foreach (var item in items)
            {
                if (updatedTranslations.ContainsKey(item))
                    continue;

                updatedTranslations.Add(item, GetTranslationIfAvailable(item));
            }
            translations = updatedTranslations;
        }

        private static Dictionary<string, string> GetTranslationIfAvailable(string key)
        {
            if (translations.ContainsKey(key))
                return translations[key];

            return new Dictionary<string, string>();
        }
    }
}
