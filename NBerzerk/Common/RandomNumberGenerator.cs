using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBerzerk
{
    public class RandomNumberGenerator
    {
        public static UInt16 seed;

        /// <summary>
        /// Linear Congruential Random Number Generator
        /// </summary>
        /// <param name="seed">seed number</param>
        /// <returns>random number</returns>
        public static UInt16 GetRandomNumber(UInt16 newSeed)
        {
            seed = (UInt16)((newSeed * 7) + 0x3153);
            return (UInt16)(seed >> 8);
        }

        public static UInt16 GetRandomNumber()
        {
            return GetRandomNumber(seed);
        }
    }
}
