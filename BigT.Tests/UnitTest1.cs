﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static BigT.Big;

namespace BigT.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private const string path = "test1.csv";
        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("Default,Finnish,Swedish");
                file.WriteLine("One,Yksi,Ett");
                file.WriteLine("Two,Kaksi,Två");
                file.WriteLine("Three,Kolme,Tre");
            }

            LoadTranslations(path);
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
            File.Delete(path);
        }

        [TestMethod]
        public void TestParsing()
        {
            Parser.RunParsing();
        }

        [TestMethod]
        public void TestDefaultTranslations()
        {
            
            Assert.AreEqual(T("One"), "One");
            Assert.AreEqual(T("Two"), "Two");
            Assert.AreEqual(T("Three"), "Three");
        }

        [TestMethod]
        public void TestFinnishTranslations()
        {
            SetLanguage("Finnish");
            Assert.AreEqual(T("One"), "Yksi");
            Assert.AreEqual(T("Two"), "Kaksi");
            Assert.AreEqual(T("Three"), "Kolme");
        }
    }
}
