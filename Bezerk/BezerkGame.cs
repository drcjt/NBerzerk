using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezerk
{
    static class BezerkGame
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            UInt16 room = RandomNumberGenerator.GetRandomNumber(0);
            string room3153 = MazeGenerator.GenerateMaze(room);
            string room3154 = MazeGenerator.GenerateMaze(0x3154);

            //UInt16 start = RandomNumberGenerator.GetRandomNumber(0);
            //UInt16 next = RandomNumberGenerator.GetRandomNumber(start);

            // Resolution is 256 x 224

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BezerkForm());
        }
    }
}
