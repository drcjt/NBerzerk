using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using GameFramework;

namespace NBezerk
{
    public class NBezerkGame : IGame
    {
        private NBezerkGameForm form;

        Image player;
        Point playerPosition = new Point(100, 100);
        int playerFrame = 0;
        bool facingRight = true;

        private string maze;

        SolidBrush wallBrush = new SolidBrush(Color.FromArgb(0, 0, 108));

        public NBezerkGame()
        {
            form = new NBezerkGameForm(this);

            form.KeyDown += new KeyEventHandler(KeyDownHandler);
            form.KeyUp += new KeyEventHandler(KeyUpHandler);

            UInt16 room = RandomNumberGenerator.GetRandomNumber(0);
            maze = MazeGenerator.GenerateMaze(room);

            player = LoadImage("player.png");
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

        public Size Resolution { get { return new Size(256, 224); } }
        public Color BackgroundColor { get { return Color.Black; } }
        public double MaxFrameRate { get { return 30; } }

        public bool ShowFramesPerSecond { get; set; }

        public void Run()
        {
            Application.Run(form);
        }

        public void Draw(System.Drawing.Graphics g)
        {
            DrawRoom(g);
            DrawPlayer(g);
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

        void DrawPlayer(Graphics g)
        {
            Rectangle destRect = new Rectangle(playerPosition, new Size(8, 16));
            Rectangle srcRect = new Rectangle(8 * playerFrame, 0, 8, 16);
            g.DrawImage(player, destRect, srcRect, GraphicsUnit.Pixel);
        }

        public void Update()
        {
            if (isKeyPressed[(int)(Keys.F11)])
            {
                ShowFramesPerSecond = !ShowFramesPerSecond;
            }

            if (isKeyPressed[(int)(Keys.Up)])
            {
                playerPosition.Y = playerPosition.Y - 1;
            }
            if (isKeyPressed[(int)(Keys.Down)])
            {
                playerPosition.Y = playerPosition.Y + 1;
            }
            if (isKeyPressed[(int)(Keys.Left)])
            {
                if (facingRight && playerFrame < 5)
                {
                    playerFrame = 5;
                    facingRight = false;
                }
                playerPosition.X = playerPosition.X - 1;
            }
            if (isKeyPressed[(int)(Keys.Right)])
            {
                if (!facingRight && playerFrame > 4)
                {
                    playerFrame = 0;
                    facingRight = true;
                }
                playerPosition.X = playerPosition.X + 1;
            }

            if (isKeyPressed[(int)(Keys.Up)] || isKeyPressed[(int)(Keys.Down)] ||
                isKeyPressed[(int)(Keys.Left)] || isKeyPressed[(int)(Keys.Right)])
            {
                playerFrame++;

                if (playerFrame % 4 == 0)
                {
                    playerFrame = playerFrame - 4;
                }
            }
            else
            {
                playerFrame = 0;
                facingRight = true;
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
