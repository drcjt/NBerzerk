using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBerzerk
{
    public class RandomNumberGenerator
    {
        /// <summary>
        /// Linear Congruential Random Number Generator
        /// </summary>
        /// <param name="seed">seed number</param>
        /// <returns>random number</returns>
        public static UInt16 GetRandomNumber(UInt16 seed)
        {
            return (UInt16)((seed * 7) + 0x3153);
        }
    }
}
