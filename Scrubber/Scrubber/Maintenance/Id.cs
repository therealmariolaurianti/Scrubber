namespace Scrubber.Maintenance
{
    public static class NextId
    {
        private static int _id = 1;

        public static int GetNext()
        {
            return _id++;
        }
    }
}