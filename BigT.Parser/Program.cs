using System;

namespace BigT
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Parsing files...");
            string startDirectory = null;
            string outputPath = null;

            if (args.Length > 0)
                startDirectory = args[0];

            if (args.Length > 1)
                outputPath = args[1];

            Parser.RunParsing(startDirectory, outputPath);
            Console.WriteLine("Done");
        }
    }
}
