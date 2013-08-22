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
using SharpDX.DirectInput;
using SharpDX.DXGI;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace NBerzerk
{
    public class NBerzerkGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;

        private PrimitiveBatch<VertexPositionColor> batch;
        private SpriteBatch spriteBatch;
        private BasicEffect effect;

        private Keyboard keyboard;

        private UInt16 roomY = 49;
        private UInt16 roomX = 83;

        private RoomObject roomObject = new RoomObject();
        private PlayerObject playerObject;
        private FPSObject fpsObject;

        bool contentLoaded = false;

        public NBerzerkGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.Resolvers.Add(new EmbeddedResourceResolver());

            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;

            keyboard = new Keyboard(new DirectInput());
            keyboard.Acquire();

            GetMaze();
        }

        private void GetMaze()
        {
            UInt16 roomNo = (UInt16)((roomY << 8) + roomX);
            roomObject.Maze = MazeGenerator.GenerateMaze(roomNo);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (!contentLoaded)
            {
                batch = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);
                spriteBatch = new SpriteBatch(GraphicsDevice);
                effect = new BasicEffect(GraphicsDevice);

                playerObject = new PlayerObject(Content.Load<Texture2D>("NBerzerk.Resources.player.png"));
                fpsObject = new FPSObject(Content.Load<SpriteFont>("NBerzerk.Resources.Arial16.tkfnt"));

                contentLoaded = true;
            }
        }

        protected override void BeginRun()
        {
            // Starts the FPS clock
            fpsObject.fpsClock.Start();
            base.BeginRun();
        }

        protected override void Initialize()
        {
            Window.Title = "NBerzerk";
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
            playerObject.Draw(spriteBatch);

            spriteBatch.End();

            fpsObject.Draw(spriteBatch);
        }
        
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = keyboard.GetCurrentState();

            fpsObject.Update(gameTime, keyboardState);

            playerObject.Update(gameTime, keyboardState);

            bool changeRoom = false;
            if (playerObject.Position.Y == 0)
            {
                roomY--;
                changeRoom = true;
            }
            if (playerObject.Position.X == 0)
            {
                roomX--;
                changeRoom = true;
            }
            if (playerObject.Position.X == 256 - 8)
            {
                roomX++;
                changeRoom = true;
            }
            if (playerObject.Position.Y == 192)
            {
                roomY++;
                changeRoom = true;
            }            

            if (changeRoom)
            {
                GetMaze();
                playerObject.Position.X = 30;
                playerObject.Position.Y = 99;
            }
        }
    }
}
