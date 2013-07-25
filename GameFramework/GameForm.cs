using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GameFramework
{
    public class GameForm : Form
    {
        private QueryPerfCounter timer;
        private Bitmap currentFrame;
        private double timeToRenderFrame = 0;

        private IGame game;

        public GameForm(IGame game)
        {
            this.game = game;

            timer = new QueryPerfCounter();
            currentFrame = new Bitmap(game.Resolution.Width, game.Resolution.Height);

            Text = "Bezerk";
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            Resize += new EventHandler(ResizeForm);
            Paint += new PaintEventHandler(GamePaint);
            
        }

        private void ResizeForm(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void GamePaint(object sender, PaintEventArgs e)
        {
            timer.Stop();
            timeToRenderFrame = timer.Duration(1);

            // Limit frame rate
            while (timeToRenderFrame < 1000000000 / game.MaxFrameRate)
            {
                timer.Stop();
                timeToRenderFrame = timer.Duration(1);
            }

            timer.Start();

            using (var g = Graphics.FromImage(currentFrame))
            {
                CreateNextFrame(g);
                DrawFPS(g);
            }

            e.Graphics.DrawImage(currentFrame, ClientRectangle);

            Invalidate();
        }

        private void CreateNextFrame(Graphics g)
        {
            game.Update();

            g.Clear(game.BackgroundColor);
            game.Draw(g);
        }

        private void DrawFPS(Graphics g)
        {
            if (game.ShowFramesPerSecond)
            {
                double framesPerSecond = Math.Round(1000000000 / timeToRenderFrame, 2, MidpointRounding.AwayFromZero);
                g.FillRectangle(new SolidBrush(Color.Black), 5, 5, 140, 22);
                g.DrawString(String.Format("FPS: {0}", framesPerSecond), new Font("Arial", 16), new SolidBrush(Color.Yellow), new PointF(5, 5));
            }
        }
    }
}
