using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bezerk
{
    public class RandomNumberGenerator
    {
        public static UInt16 GetRandomNumber(UInt16 seed)
        {
            return (UInt16)((seed * 7) + 0x3153);
        }
    }
}
