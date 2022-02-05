using System;

namespace CryptoQuoteAPI
{
    public static class Utils
    {
        private static Random rnd = new Random();
        //https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        public static decimal GetRandomDecimal(decimal min, decimal max)
        {
            var numRandom = Convert.ToDecimal(rnd.NextDouble()) * (max - min) + min;

            return numRandom; 
        }
    }
}
