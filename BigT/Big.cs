using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BigT
{
    public static class Big
    {
        private const string DefaultLanguageIdentifier = "Default";

        private static IList<string> _languages = new List<string>() { DefaultLanguageIdentifier };
        private static Dictionary<string, Dictionary<string, string>> _translations = new Dictionary<string, Dictionary<string, string>>();
        private static string _activeLanguage = DefaultLanguageIdentifier;

        public static string T(string text)
        {
            return T(text, _activeLanguage);
        }

        public static string T(string text, string language)
        {
            if (_translations.ContainsKey(text) && _translations[text].ContainsKey(_activeLanguage))
                return _translations[text][_activeLanguage];
            return text;
        }

        public static void SetLanguage(string language)
        {
            _activeLanguage = language;
        }

        public static void LoadTranslations(string path = Parser.DefaultOutputFile)
        {
            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            {
                var data = Csv.ParseValues(reader, ',', '"');
                _languages = data.First();   
                _translations = MapDefaultValueToTranslations(data.Skip(1), _languages);
            }
        }

        private static Dictionary<string, Dictionary<string, string>> MapDefaultValueToTranslations(IEnumerable<IList<string>> rows,
            IList<string> languageList)
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

        private static Dictionary<string, string> MapLanguagesToTranslations(IList<string> languageList, IList<string> row)
        {
            var values = new Dictionary<string, string>();
            for (var i = 1; i < row.Count; i++)
            {
                values.Add(languageList.ElementAt(i), row[i]);
            }
            return values;
        }

        public static void SaveTranslations(string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (var file = new StreamWriter(path))
            {
                WriteLanguages(file);
                WriteTranslations(file);
            }
        }

        private static void WriteLanguages(TextWriter file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            for (var i = 0; i < _languages.Count; i++)
            {
                if (i>0)
                    file.Write(",");

                file.Write(ConvertToCsvSafe(_languages[i]));
            }
            file.WriteLine();
        }

        private static void WriteTranslations(TextWriter file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            foreach (var translatedString in _translations)
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
            _translations = updatedTranslations;
        }

        private static Dictionary<string, string> GetTranslationIfAvailable(string key)
        {
            return _translations.ContainsKey(key) ? _translations[key] : new Dictionary<string, string>();
        }
    }
}
