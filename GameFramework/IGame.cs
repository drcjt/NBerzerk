using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameFramework
{
    public interface IGame
    {
        Size Resolution { get; }
        Color BackgroundColor { get; }

        double MaxFrameRate { get; }

        bool ShowFramesPerSecond { get; }

        void Run();

        void Draw(Graphics g);
        void Update();
    }
}
