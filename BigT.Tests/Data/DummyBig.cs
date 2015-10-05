namespace BigT.Tests.Data
{
    public static class DummyBig
    {
        public static string T(string parameter)
        {
            return parameter;
        }

        public static void AnotherDummyFunction()
        {
            T("This should match");
            T("AND this should match too");
        }
    }
}
