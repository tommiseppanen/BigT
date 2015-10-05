using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigT.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestParsing()
        {
            Parser.RunParsing();
        }

        [TestMethod]
        public void TestTranslationLoading()
        {
            Big.LoadTranslations(@"translations.csv");
        }
    }
}
