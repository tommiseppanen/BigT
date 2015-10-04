using static BigT.Big;

namespace BigT.Tests.Data
{
    class DummyClass1
    {
        public void DummyFunction()
        {
            T("Should match");
            var test = T(@"Multiline
                            string
                            which should match");

            string normal = T("Normal, should match");
            string full = Big.T(@"Normal, should match");

            var anotherTest = TestFunction(T("Pick me"));
            BigT.Big.T("And me");
            string quoted = Big.T(@"Normal, ""should"" match");

            TestFunctionT("Not this one");
            DummyBig.T("Neither this");

        }

        private string TestFunction(string parameter)
        {
            return parameter;
        }

        private string TestFunctionT(string parameter)
        {
            return parameter;
        }
    }
}
