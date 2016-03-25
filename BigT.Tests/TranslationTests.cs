using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static BigT.Big;

namespace BigT.Tests
{
    [TestClass]
    public class TranslationTests
    {
        private const string Path = "test1.csv";
        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            
            using (var file = new StreamWriter(Path))
            {
                file.WriteLine("Default,Finnish,Swedish");
                file.WriteLine("One,Yksi,Ett");
                file.WriteLine("Two,Kaksi,Två");
                file.WriteLine("Three,Kolme,Tre");
            }

            LoadTranslations(Path);
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
            File.Delete(Path);
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
