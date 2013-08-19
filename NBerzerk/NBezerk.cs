using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NBerzerk
{
    static class NBerzerk
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            NBerzerkGame game = new NBerzerkGame();
            game.Run();
        }
    }
}
