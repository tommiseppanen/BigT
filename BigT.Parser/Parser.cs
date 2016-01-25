using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BigT
{
    public class Parser
    {
        private const string identifier = "*.cs";
        private const string outputFile = "translations.csv";

        //Regex pattern parts
        private const string functionSeparator = @"[\s,\(\{]";
        private const string functionName = @"(?:T|Big\.T|BigT\.Big\.T)";
        private const string optionalWhitespace = @"\s*";
        private const string captureGroup = @"(@?)""(.*?)""";

        private const string pattern = functionSeparator + functionName
            + optionalWhitespace + @"\(" + optionalWhitespace 
            + captureGroup + optionalWhitespace + @"\)";

        public static void RunParsing(string startDirectory = null, string outputPath = null)
        {
            var directory = GetDirectory(startDirectory);
            var filePath = GetFilePath(directory, outputPath);

            if (File.Exists(filePath))
                Big.LoadTranslations(filePath);

            Regex matchPattern = new Regex(pattern, RegexOptions.Singleline);
            var translationsFromParsing = ReadStringsRecursively(directory, identifier, matchPattern);
            Big.UpdateTranslations(translationsFromParsing);
            Big.SaveTranslations(filePath);            
        }

        private static string GetDirectory(string startDirectory)
        {
            if (startDirectory != null)
                return startDirectory;

            return Directory.GetCurrentDirectory();
        }

        private static string GetFilePath(string directory, string outputPath)
        {
            if (outputPath != null)
                return outputPath;

            return Path.Combine(directory, outputFile);
        }

        private static List<String> ReadStringsRecursively(string filePath, string fileIdentifier, Regex matchPattern)
        {  
            string[] files = Directory.GetFiles(filePath, fileIdentifier);
            var translations = GetStringsFromFiles(files, matchPattern);
            var subDirectories = Directory.GetDirectories(filePath);
            foreach (var directory in subDirectories)
            {
                translations.AddRange(ReadStringsRecursively(directory, fileIdentifier, matchPattern));
            }

            return translations;
        }

        private static List<String> GetStringsFromFiles(string[] files, Regex matchPattern)
        {
            var translations = new List<String>();
            foreach (var file in files)
            {
                string fileContent = File.ReadAllText(file);
                Match m = matchPattern.Match(fileContent);
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