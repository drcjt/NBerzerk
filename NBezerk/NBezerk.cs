using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NBezerk
{
    static class NBezerk
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            NBezerkGame game = new NBezerkGame();
            game.Run();
        }
    }
}
