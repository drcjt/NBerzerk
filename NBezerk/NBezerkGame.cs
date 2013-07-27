using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using GameFramework;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace NBezerk
{
    public class NBezerkGame : IGame
    {
        private NBezerkGameForm form;

        FPSRenderer fpsRenderer;

        SharpDX.Direct2D1.Bitmap player;
        DrawingPoint playerPosition = new DrawingPoint(100, 100);
        int playerFrame = 0;
        bool facingRight = true;

        private string maze;

        SolidColorBrush wallBrush;

        public NBezerkGame()
        {
            form = new NBezerkGameForm(this);
            form.Text = "NBezerk";

            form.KeyDown += new KeyEventHandler(KeyDownHandler);
            form.KeyUp += new KeyEventHandler(KeyUpHandler);

            UInt16 room = RandomNumberGenerator.GetRandomNumber(0);
            maze = MazeGenerator.GenerateMaze(room);
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

        public DrawingSize Resolution { get { return new DrawingSize(256, 224); } }
        public Color4 BackgroundColor { get { return Color.Black; } }
        public double MaxFrameRate { get { return 3000; } }

        public bool ShowFramesPerSecond { get; set; }

        public void Run()
        {
            form.Run();
        }

        public void LoadContent(RenderTarget windowRenderTarget)
        {
            wallBrush = new SolidColorBrush(windowRenderTarget, new SharpDX.Color4(0, 0, 108, 255));
            player = BitmapExtensions.LoadFromFile(windowRenderTarget, "NBezerk.Resources.player.png");
            fpsRenderer = new FPSRenderer(this);
        }

        public void Render(RenderTarget renderTarget)
        {
            fpsRenderer.Render(renderTarget);
            RenderRoom(renderTarget);
            RenderPlayer(renderTarget);
        }

        void RenderRoom(RenderTarget windowRenderTarget)
        {
            // Draw top walls
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(4, 0, 4 + 99, 0 + 4), wallBrush);
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(152, 0, 152 + 99, 0 + 4), wallBrush);

            // Draw bottom walls
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(4, 204, 4 + 99, 204 + 4), wallBrush);
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(152, 204, 152 + 99, 204 + 4), wallBrush);

            // Draw left walls
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(4, 0, 4 + 4, 0 + 71), wallBrush);
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(4, 136, 4 + 4, 136 + 71), wallBrush);

            // Draw right walls
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(248, 0, 248 + 4, 0 + 71), wallBrush);
            windowRenderTarget.FillRectangle(new SharpDX.RectangleF(248, 136, 248 + 4, 136 + 71), wallBrush);

            for (int pillarIndex = 0; pillarIndex < 8; pillarIndex++)
            {
                DrawingPoint pillarLocation = new DrawingPoint(56 + (pillarIndex % 4) * 48, pillarIndex < 4 ? 68 : 136);

                char wallDirection = maze[pillarIndex];

                SharpDX.RectangleF wallRectangle = new SharpDX.RectangleF();
                wallRectangle.Left = (wallDirection == 'W') ? pillarLocation.X - 52 : pillarLocation.X;
                wallRectangle.Top = (wallDirection == 'N') ? pillarLocation.Y - 67 : pillarLocation.Y;
                wallRectangle.Right = wallRectangle.Left + ((wallDirection == 'N' || wallDirection == 'S') ? 4 : 52);
                wallRectangle.Bottom = wallRectangle.Top + ((wallDirection == 'N' || wallDirection == 'S') ? 71 : 4);

                windowRenderTarget.FillRectangle(wallRectangle, wallBrush);
            }
        }

        void RenderPlayer(RenderTarget windowRenderTarget)
        {
            SharpDX.RectangleF destinationRectangle = new SharpDX.RectangleF(playerPosition.X, playerPosition.Y, playerPosition.X + 8, playerPosition.Y + 16);
            SharpDX.RectangleF sourceRectangle = new SharpDX.RectangleF(8 * playerFrame, 0, 8 * playerFrame + 8, 16);
            windowRenderTarget.DrawBitmap(player, destinationRectangle, 1.0f, BitmapInterpolationMode.Linear, sourceRectangle);
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
    }
}
