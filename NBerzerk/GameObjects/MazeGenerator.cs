using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBerzerk
{
    public class MazeGenerator
    {
        /// <summary>
        /// Generate a berzerk maze based on a room's coordinates
        /// </summary>
        /// <param name="room">room's coordinates as unsigned 16 bit number, top 8 bits = </param>
        /// <returns></returns>
        public static string GenerateMaze(UInt16 room, out int[,] walls)
        {
            // Walls in each of screen cells, 5 wide by 3 high.
            // bit 1 = wall on left
            // bit 2 = wall on right
            // bit 3 = wall on top
            // bit 4 = wall on bottom.
            walls = new int[5, 3];

            RandomNumberGenerator.seed = room;

            StringBuilder maze = new StringBuilder(8);

            for (var pillarIndex = 0; pillarIndex < 8; pillarIndex++)
            {
                RandomNumberGenerator.GetRandomNumber();
                UInt16 pillarValue = RandomNumberGenerator.GetRandomNumber();
                char nextWall = GetNextWall(pillarValue);
                maze.Append(nextWall);

                if (nextWall == 'N')
                {
                    walls[pillarIndex % 4, pillarIndex / 4] |= 2;
                    walls[(pillarIndex % 4) + 1, pillarIndex / 4] |= 1;   
                }
                if (nextWall == 'S')
                {
                    walls[pillarIndex % 4, (pillarIndex / 4) + 1] |= 2;
                    walls[(pillarIndex % 4) + 1, (pillarIndex / 4) + 1] |= 1;   
                }
                if (nextWall == 'E')
                {
                    walls[(pillarIndex % 4) + 1, pillarIndex / 4] |= 8;
                    walls[(pillarIndex % 4) + 1, (pillarIndex / 4) + 1] |= 4;   
                }
                if (nextWall == 'W')
                {
                    walls[pillarIndex % 4, pillarIndex / 4] |= 8;
                    walls[pillarIndex % 4, (pillarIndex / 4) + 1] |= 4;   
                }

                for (int column = 0; column < 5; column++)
                {
                    walls[column, 0] |= 4;
                    walls[column, 2] |= 8;
                }

                for (int row = 0; row < 3; row++)
                {
                    walls[0, row] |= 1;
                    walls[4, row] |= 2;
                }
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
            var wallBits = (UInt16)(pillarValue & 3);

            return "NSEW"[wallBits];
        }
    }
}
