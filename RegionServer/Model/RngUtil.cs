using System;

namespace RegionServer.Model
{
    public class RngUtil
    {
        private static readonly Random rng = new Random();

        //TODO: write RngUtil class with rng.int, rng.float, rng.intBias, rng.floatBias 


        public static int hundredRoll()
        {
            return rng.Next(0, 100);
        }

        /// <summary>
        /// Returns a random integer in the range from min to max INCLUSIVE.
        /// </summary>
        /// <returns></returns>
        public static int intRange(int min, int max)
        {
            return rng.Next(min, max+1);
        }

        public static int intMax(int max)
        {
            return rng.Next(0, max+1);
        }

    }
} 
