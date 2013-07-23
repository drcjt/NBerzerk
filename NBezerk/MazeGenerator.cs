using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBezerk
{
    public class MazeGenerator
    {
        /// <summary>
        /// Generate a bezerk maze based on a room's coordinates
        /// </summary>
        /// <param name="room">room's coordinates as unsigned 16 bit number, top 8 bits = </param>
        /// <returns></returns>
        public static string GenerateMaze(UInt16 room)
        {
            UInt16 pillarValue = room;

            StringBuilder maze = new StringBuilder(8);

            for (int pillarIndex = 0; pillarIndex < 8; pillarIndex++)
            {
                pillarValue = RandomNumberGenerator.GetRandomNumber(RandomNumberGenerator.GetRandomNumber(pillarValue));
                maze.Append(GetNextWall(pillarValue));
            }

            return maze.ToString();
        }

        /// <summary>
        /// Convert the 16 bit number generated from the random number generator
        /// into a wall direction. This is done by taking the high 8 bits and
        /// looking at the 2 low bits of this.
        /// </summary>
        /// <param name="pillarValue">pillar value to get wall direction from</param>
        /// <returns>wall direction as a character, 'N'orth, 'S'outh, 'E'ast, or 'W'est</returns>
        private static char GetNextWall(UInt16 pillarValue)
        {
            UInt16 wallBits = (UInt16)(pillarValue >> 8);
            wallBits = (UInt16)(wallBits & 3);

            return "NSEW"[wallBits];
        }
    }
}
