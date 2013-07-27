using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using SharpDX;
using SharpDX.Direct2D1;

namespace GameFramework
{
    public interface IGame
    {
        DrawingSize Resolution { get; }
        Color4 BackgroundColor { get; }

        double MaxFrameRate { get; }

        bool ShowFramesPerSecond { get; }

        void Run();

        void LoadContent(RenderTarget windowRenderTarget);
        void Render(RenderTarget windowRenderTarget);
        void Update();
    }
}
