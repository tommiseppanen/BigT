using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigT.Tests
{
    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void TestParsing()
        {
            Parser.RunParsing();
        }
    }
}
