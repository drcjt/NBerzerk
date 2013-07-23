using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezerk
{
    public partial class BezerkForm : Form
    {
        Bitmap backBuffer;

        string maze;

        public BezerkForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            UInt16 room = RandomNumberGenerator.GetRandomNumber(0);
            maze = MazeGenerator.GenerateMaze(room);

            Timer GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(GameTick);
            GameTimer.Start();

            Load += new EventHandler(CreateBackBuffer);
            Paint += new PaintEventHandler(GamePaint);
        }

        void CreateBackBuffer(object sender, EventArgs e)
        {
            if (backBuffer != null)
            {
                backBuffer.Dispose();
            }

            backBuffer = new Bitmap(256, 224);
        }

        void GamePaint(object sender, PaintEventArgs e)
        {
            if (backBuffer != null)
            {
                //e.Graphics.DrawImageUnscaled(backBuffer, Point.Empty);
                e.Graphics.DrawImage(backBuffer, ClientRectangle);
            }
        }

        private static Point[] pillarLocations = new Point[8] {
            new Point(56, 68),
            new Point(104, 68),
            new Point(152, 68),
            new Point(200, 68),
            new Point(56, 136),
            new Point(104, 136),
            new Point(152, 136),
            new Point(200, 136)
        };

        void DrawRoom()
        {
            using (var g = Graphics.FromImage(backBuffer))
            {
                g.Clear(Color.Black);

                // Draw top walls
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 4, 0, 99, 4);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 152, 0, 99, 4);

                // Draw bottom walls
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 4, 204, 99, 4);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 152, 204, 99, 4);

                // Draw left walls
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 4, 0, 4, 71);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 4, 136, 4, 71);

                // Draw right walls
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 248, 0, 4, 71);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), 248, 136, 4, 71);

                for (int pillarIndex = 0; pillarIndex < 8; pillarIndex++)
                {
                    // Wall segments are always 52 pixels long, 4 deep
                    Rectangle wallRectangle = new Rectangle();

                    char wallDirection = maze[pillarIndex];

                    switch (wallDirection)
                    {
                        case 'N':
                            wallRectangle.X = pillarLocations[pillarIndex].X;
                            wallRectangle.Y = pillarLocations[pillarIndex].Y - 67;
                            wallRectangle.Width = 4;
                            wallRectangle.Height = 71;

                            break;
                        case 'S':
                            wallRectangle.Location = pillarLocations[pillarIndex];
                            wallRectangle.Width = 4;
                            wallRectangle.Height = 71;
                            break;
                        case 'E':
                            wallRectangle.Location = pillarLocations[pillarIndex];
                            wallRectangle.Width = 52;
                            wallRectangle.Height = 4;
                            break;
                        case 'W':
                            wallRectangle.X = pillarLocations[pillarIndex].X - 52;
                            wallRectangle.Y = pillarLocations[pillarIndex].Y;
                            wallRectangle.Width = 52;
                            wallRectangle.Height = 4;
                            break;
                    }

                    g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 108)), wallRectangle);
                }
            }
        }

        void GameTick(object sender, EventArgs e)
        {
            if (backBuffer != null)
            {
                DrawRoom();
                Invalidate();
            }
        }
    }
}
