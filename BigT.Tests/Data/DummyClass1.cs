using static BigT.Big;

namespace BigT.Tests.Data
{
    internal class DummyClass1
    {
        public void DummyFunction()
        {
            T("Should match");
            var test = T(@"Multiline
                            string
                            which should match");

            string normal = T("Normal, should match");
            var full = Big.T(@"Normal, should match");

            var multipart = T("Multiparts should" + " match too");

            var multipartAndMultiline = T("Also multiline" 
                                        + " multiparts "
                                        + "might be needed");

            var anotherTest = TestFunction(T("Pick me"));
            BigT.Big.T("And me");
            var quoted = Big.T(@"Normal, ""should"" match");

            TestFunctionT("Not this one");
            DummyBig.T("Neither this");
        }

        private static string TestFunction(string parameter)
        {
            return parameter;
        }

        private static string TestFunctionT(string parameter)
        {
            return parameter;
        }
    }
}
