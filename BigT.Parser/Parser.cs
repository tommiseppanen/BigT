using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BigT
{
    public class Parser
    {
        private const string Identifier = "*.cs";
        private const string OutputFile = "translations.csv";

        //Regex pattern parts
        private const string FunctionSeparator = @"[\s,\(\{]";
        private const string FunctionName = @"(?:T|Big\.T|BigT\.Big\.T)";
        private const string OptionalWhitespace = @"\s*";
        private const string CaptureGroup = @"(@?)""(.*?)""";

        private const string Pattern = FunctionSeparator + FunctionName
            + OptionalWhitespace + @"\(" + OptionalWhitespace 
            + CaptureGroup + OptionalWhitespace + @"\)";

        public static void RunParsing(string startDirectory = null, string outputPath = null)
        {
            var directory = GetDirectory(startDirectory);
            var filePath = GetFilePath(directory, outputPath);

            if (File.Exists(filePath))
                Big.LoadTranslations(filePath);

            var matchPattern = new Regex(Pattern, RegexOptions.Singleline);
            var translationsFromParsing = ReadStringsRecursively(directory, Identifier, matchPattern);
            Big.UpdateTranslations(translationsFromParsing);
            Big.SaveTranslations(filePath);            
        }

        private static string GetDirectory(string startDirectory)
        {
            return startDirectory ?? Directory.GetCurrentDirectory();
        }

        private static string GetFilePath(string directory, string outputPath)
        {
            if (outputPath != null)
                return outputPath;

            return Path.Combine(directory, OutputFile);
        }

        private static IEnumerable<string> ReadStringsRecursively(string filePath, string fileIdentifier, Regex matchPattern)
        {  
            var files = Directory.GetFiles(filePath, fileIdentifier);
            var translations = GetStringsFromFiles(files, matchPattern);
            var subDirectories = Directory.GetDirectories(filePath);
            foreach (var directory in subDirectories)
            {
                translations.AddRange(ReadStringsRecursively(directory, fileIdentifier, matchPattern));
            }
            return translations;
        }

        private static List<string> GetStringsFromFiles(IEnumerable<string> files, Regex matchPattern)
        {
            var translations = new List<string>();
            foreach (var file in files)
            {
                var fileContent = File.ReadAllText(file);
                var m = matchPattern.Match(fileContent);
                while (m.Success)
                {
                    if (m.Groups[1].Length == 0)
                    {
                        var parsed = Regex.Replace(m.Groups[2].ToString(), @""".*?""", "", RegexOptions.Singleline);
                        translations.Add(parsed);
                    }
                    else
                    {
                        translations.Add(m.Groups[2].ToString().Replace(@"""""", @""""));
                    }                    
                    m = m.NextMatch();
                }
            }
            return translations;
        }
    }
}