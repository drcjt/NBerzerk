using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX.DXGI;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace NBezerk
{
    public class NBezerkGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;

        private PrimitiveBatch<VertexPositionColor> batch;
        private SpriteBatch spriteBatch;
        private BasicEffect effect;
        private SpriteFont arial16BMFont;

        private Texture2D player;

        private Keyboard keyboard;

        private readonly Stopwatch fpsClock;
        private int frameCount;
        private string fpsText = "";

        Bitmap font;
        DrawingPoint playerPosition = new DrawingPoint(30, 99);
        int playerFrame = 0;
        bool facingRight = true;

        int score = 1270;

        private string maze;
        private UInt16 roomY = 49;
        private UInt16 roomX = 83;

        private RoomObject roomObject = new RoomObject();

        public NBezerkGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.Resolvers.Add(new EmbeddedResourceResolver());

            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;

            keyboard = new Keyboard(new DirectInput());
            keyboard.Acquire();

            GetMaze();

            fpsClock = new Stopwatch();
        }

        private void GetMaze()
        {
            UInt16 roomNo = (UInt16)((roomY << 8) + roomX);
            roomObject.Maze = MazeGenerator.GenerateMaze(roomNo);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            batch = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);

            player = Content.Load<Texture2D>("NBezerk.Resources.player.png");

            arial16BMFont = Content.Load<SpriteFont>("NBezerk.Resources.Arial16.tkfnt");
        }

        protected override void BeginRun()
        {
            // Starts the FPS clock
            fpsClock.Start();
            base.BeginRun();
        }

        protected override void Initialize()
        {
            Window.Title = "NBezerk";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            base.Initialize();
        }

        public DrawingSize Resolution { get { return new DrawingSize(256, 224); } }
        public Color BackgroundColor { get { return Color.Black; } }
        public double MaxFrameRate { get { return 3000; } }

        public bool ShowFramesPerSecond { get; set; }

        /*
        void RenderScore(RenderTarget renderTarget)
        {
            string scoreAsText = String.Format("{0,6:#####0}", score);
            RenderText(scoreAsText, renderTarget, new DrawingPoint(1, 213));
        }
        */

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix scaleMatrix = Matrix.Scaling(GraphicsDevice.Viewport.Width / Resolution.Width, GraphicsDevice.Viewport.Height / Resolution.Height, 1);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, scaleMatrix);

            roomObject.Draw(spriteBatch);
            RenderPlayer();

            spriteBatch.End();

            // Update the FPS text
            frameCount++;
            if (fpsClock.ElapsedMilliseconds > 1000.0f)
            {
                fpsText = string.Format("{0:F2} FPS", (float)frameCount * 1000 / fpsClock.ElapsedMilliseconds);
                frameCount = 0;
                fpsClock.Restart();
            }

            // Render the text
            spriteBatch.Begin();
            spriteBatch.DrawString(arial16BMFont, fpsText, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }

        void RenderPlayer()
        {
            Rectangle destinationRectangle = new Rectangle(playerPosition.X, playerPosition.Y, playerPosition.X + 7, playerPosition.Y + 15);
            Rectangle sourceRectangle = new Rectangle(8 * playerFrame, 0, 8 * playerFrame + 7, 15);

            spriteBatch.Draw(player, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.One, SpriteEffects.None, 0f);
        }        
        
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = keyboard.GetCurrentState();

            if (keyboardState.IsPressed(Key.F11))
            {
                ShowFramesPerSecond = !ShowFramesPerSecond;
            }

            if (gameTime.FrameCount % 2 == 0)
            {

                if (keyboardState.IsPressed(Key.UpArrow))
                {
                    playerPosition.Y = playerPosition.Y - 1;
                }
                if (keyboardState.IsPressed(Key.Down))
                {
                    playerPosition.Y = playerPosition.Y + 1;
                }
                if (keyboardState.IsPressed(Key.Left))
                {
                    if (facingRight && playerFrame < 5)
                    {
                        playerFrame = 5;
                        facingRight = false;
                    }
                    playerPosition.X = playerPosition.X - 1;
                }
                if (keyboardState.IsPressed(Key.Right))
                {
                    if (!facingRight && playerFrame > 4)
                    {
                        playerFrame = 0;
                        facingRight = true;
                    }
                    playerPosition.X = playerPosition.X + 1;
                }
            }

            if (keyboardState.IsPressed(Key.UpArrow) || keyboardState.IsPressed(Key.Down) ||
                keyboardState.IsPressed(Key.Left) || keyboardState.IsPressed(Key.Right))
            {
                if (gameTime.FrameCount % 5 == 0)
                {
                    playerFrame++;
                }

                if (playerFrame > 0 && playerFrame % 4 == 0)
                {
                    playerFrame = playerFrame - 4;
                }
            }
            else
            {
                playerFrame = 0;
                facingRight = true;
            }

            bool changeRoom = false;
            if (playerPosition.Y == 0)
            {
                roomY--;
                changeRoom = true;
            }
            if (playerPosition.X == 0)
            {
                roomX--;
                changeRoom = true;
            }
            if (playerPosition.X == 256 - 8)
            {
                roomX++;
                changeRoom = true;
            }
            if (playerPosition.Y == 192)
            {
                roomY++;
                changeRoom = true;
            }

            if (changeRoom)
            {
                GetMaze();
                playerPosition.X = 30;
                playerPosition.Y = 99;
            }
        }
    }
}
