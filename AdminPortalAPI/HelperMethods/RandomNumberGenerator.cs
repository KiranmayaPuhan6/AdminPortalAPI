namespace UserAPI.HelperMethods
{
    public static class RandomNumberGenerator
    {
        // Instantiate random number generator.  
        private static readonly Random _random = new Random();         // Generates a random number within a range.      
        public static int Generate(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
