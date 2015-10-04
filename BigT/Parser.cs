using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BigT
{
    public class Parser
    {
        private const string path = "";
        private const string identifier = "*.cs";

        //Regex pattern parts
        private const string functionSeparator = @"[\s,\(\{]";
        private const string functionName = @"(?:T|Big\.T|BigT\.Big\.T)";
        private const string optionalWhitespace = @"\s*";
        private const string captureGroup = @"@?""(.*?)""";

        private const string pattern = functionSeparator + functionName
            + optionalWhitespace + @"\(" + optionalWhitespace 
            + captureGroup + optionalWhitespace + @"\)";

        public static void RunParsing()
        {
            var currentPath = Directory.GetCurrentDirectory();
            var translations = ReadStrings(currentPath + path, identifier, pattern);
            foreach (var entry in translations)
            {
                Console.WriteLine(entry);
            }
        }

        private static List<String> ReadStrings(string filePath, string fileIdentifier, string matchPattern)
        {
            
            string[] files = Directory.GetFiles(filePath, fileIdentifier);
            var translations = new List<String>();
            Regex r = new Regex(matchPattern, RegexOptions.Singleline);
            foreach (var file in files)
            {
                string fileContent = File.ReadAllText(file);
                Match m = r.Match(fileContent);
                while (m.Success)
                {
                    translations.Add(m.Groups[1].ToString());
                    m = m.NextMatch();
                } 
            }

            var subDirectories = Directory.GetDirectories(filePath);
            foreach (var directory in subDirectories)
            {
                translations.AddRange(ReadStrings(directory, fileIdentifier, matchPattern));
            }

            return translations;
        }
    }
}