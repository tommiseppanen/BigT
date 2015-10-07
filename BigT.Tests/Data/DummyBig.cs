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
            T("This should actually match");
            T("AND this should match too");
            T(@"Special characters "" §½!@#£¤$%&/{([)]=}+?\´`åÅ¨^~*'öÖäÄ,;.:-_<>|");
            T("");
            T("short");
            T(" ");
        }
    }
}
