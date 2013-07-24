using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NBezerk
{
    public partial class NBezerkForm : Form
    {
        SolidBrush wallBrush = new SolidBrush(Color.FromArgb(0, 0, 108));

        Bitmap backBuffer;
        Image player;
        Point playerPosition = new Point(100, 100);

        string maze;

        public NBezerkForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            player = LoadImage("player.png");

            UInt16 room = RandomNumberGenerator.GetRandomNumber(0);
            maze = MazeGenerator.GenerateMaze(room);

            Timer GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(GameTick);
            GameTimer.Start();

            KeyDown += new KeyEventHandler(KeyDownHandler);
            KeyUp += new KeyEventHandler(KeyUpHandler);

            Load += new EventHandler(CreateBackBuffer);
            Paint += new PaintEventHandler(GamePaint);
        }

        bool[] isKeyPressed = new bool[Enum.GetNames(typeof(Keys)).Length];

        void KeyDownHandler(object sender, KeyEventArgs e)
        {
            isKeyPressed[(int)(e.KeyCode)] = true;
        }

        void KeyUpHandler(object sender, KeyEventArgs e)
        {
            isKeyPressed[(int)(e.KeyCode)] = false;
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

        void DrawPlayer(Graphics g)
        {
            Rectangle destRect = new Rectangle(playerPosition, new Size(8, 16));
            Rectangle srcRect = new Rectangle(0, 0, 8, 16);
            g.DrawImage(player, destRect, srcRect, GraphicsUnit.Pixel);
            //Image i = new 
            //
        }

        void DrawRoom(Graphics g)
        {
            // Draw top walls
            g.FillRectangle(wallBrush, 4, 0, 99, 4);
            g.FillRectangle(wallBrush, 152, 0, 99, 4);

            // Draw bottom walls
            g.FillRectangle(wallBrush, 4, 204, 99, 4);
            g.FillRectangle(wallBrush, 152, 204, 99, 4);

            // Draw left walls
            g.FillRectangle(wallBrush, 4, 0, 4, 71);
            g.FillRectangle(wallBrush, 4, 136, 4, 71);

            // Draw right walls
            g.FillRectangle(wallBrush, 248, 0, 4, 71);
            g.FillRectangle(wallBrush, 248, 136, 4, 71);

            for (int pillarIndex = 0; pillarIndex < 8; pillarIndex++)
            {
                Point pillarLocation = new Point(56 + (pillarIndex % 4) * 48, pillarIndex < 4 ? 68 : 136);

                char wallDirection = maze[pillarIndex];

                Rectangle wallRectangle = new Rectangle();
                wallRectangle.X = (wallDirection == 'W') ? pillarLocation.X - 52 : pillarLocation.X;
                wallRectangle.Y = (wallDirection == 'N') ? pillarLocation.Y - 67 : pillarLocation.Y;
                wallRectangle.Width = (wallDirection == 'N' || wallDirection == 'S') ? 4 : 52;
                wallRectangle.Height = (wallDirection == 'N' || wallDirection == 'S') ? 71 : 4;

                g.FillRectangle(wallBrush, wallRectangle);
            }
        }

        void GameTick(object sender, EventArgs e)
        {
            if (backBuffer != null)
            {
                using (var g = Graphics.FromImage(backBuffer))
                {
                    g.Clear(Color.Black);

                    DrawRoom(g);
                    DrawPlayer(g);
                }

                Invalidate();
            }

            if (isKeyPressed[(int)(Keys.Up)])
            {
                playerPosition.Y = playerPosition.Y - 3;
            }
            if (isKeyPressed[(int)(Keys.Down)])
            {
                playerPosition.Y = playerPosition.Y + 3;
            }
            if (isKeyPressed[(int)(Keys.Left)])
            {
                playerPosition.X = playerPosition.X - 3;
            }
            if (isKeyPressed[(int)(Keys.Right)])
            {
                playerPosition.X = playerPosition.X + 3;
            }
        }

        Image LoadImage(string imageFile)
        {
            var thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            string[] names = thisExe.GetManifestResourceNames();
            System.IO.Stream file = thisExe.GetManifestResourceStream("NBezerk.Resources." + imageFile);
            return Image.FromStream(file);
        }
    }
}
