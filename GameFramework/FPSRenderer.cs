using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace GameFramework
{
    public class FPSRenderer
    {
        Stopwatch clock;
        double totalTime;
        long frameCount;
        double measuredFPS;

        private IGame game;

        SharpDX.DirectWrite.Factory dwFactory = new SharpDX.DirectWrite.Factory();

        public FPSRenderer(IGame game)
        {
            this.game = game;

            clock = Stopwatch.StartNew();
        }

        public void Render(RenderTarget renderTarget)
        {
            frameCount++;
            var timeElapsed = (double)clock.ElapsedTicks / Stopwatch.Frequency; ;
            totalTime += timeElapsed;
            if (totalTime >= 1.0f)
            {
                measuredFPS = (double)frameCount / totalTime;
                frameCount = 0;
                totalTime = 0.0;
            }

            if (game.ShowFramesPerSecond)
            {
                TextFormat textFormat = new TextFormat(dwFactory, "Calibri", 20) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center };
                using (SolidColorBrush brush = new SolidColorBrush(renderTarget, Color4.White))
                {
                    renderTarget.DrawText(string.Format("{0:F2} FPS ({1:F1} ms)", measuredFPS, timeElapsed * 1000.0), textFormat, new SharpDX.RectangleF(8, 8, 8 + 256, 8 + 16), brush);
                }
            }

            clock.Restart();
        } 
    }
}
