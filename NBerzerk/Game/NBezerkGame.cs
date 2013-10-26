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

        static public Keyboard keyboard;

        private UInt16 roomY = 49;
        private UInt16 roomX = 83;

        private RoomObject roomObject = new RoomObject();
        private PlayerObject playerObject = new PlayerObject();
        private FPSObject fpsObject = new FPSObject();

        private ComponentFramework.TextRendererObject textRendererObject = new ComponentFramework.TextRendererObject();

        bool contentLoaded = false;

        public NBerzerkGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.Resolvers.Add(new EmbeddedResourceResolver());

            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;

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

                playerObject.LoadContent(Content);
                fpsObject.LoadContent(Content);

                textRendererObject.LoadContent(Content);

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
            Window.AllowUserResizing = false;
            (this.Window.NativeWindow as Form).Icon = new System.Drawing.Icon(this.GetType(), "NBerzerk.ico");

            (this.Window.NativeWindow as Form).ClientSize = new System.Drawing.Size(256 * 2, 224 * 2);
            (this.Window.NativeWindow as Form).MaximizeBox = false;

            base.Initialize();
        }

        public Size2 Resolution { get { return new Size2(256, 224); } }

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

            Matrix scaleMatrix = Matrix.Scaling(2.0f, 2.0f, 1.0f);
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied, GraphicsDevice.SamplerStates.PointClamp, null, GraphicsDevice.RasterizerStates.CullNone, null, scaleMatrix);

            roomObject.Draw(spriteBatch);
            playerObject.Draw(spriteBatch);
            
            /*
            textRendererObject.DrawText("High Scores", new Vector2(88, 1), new Color(0, 255, 0, 255), spriteBatch);
            textRendererObject.DrawText("Push 1 Player Start Button", new Vector2(20, 190), new Color(255, 0, 0, 255), spriteBatch);

            textRendererObject.DrawText(string.Format("{0,6:#####0}", 150), new Vector2(1, 213), new Color(0, 255, 0, 255), spriteBatch);
            

            textRendererObject.DrawText("34", new Vector2(120, 213), new Color(108, 108, 108, 255), spriteBatch);
            */

            spriteBatch.End();

            spriteBatch.Begin();
            fpsObject.Draw(spriteBatch);
            spriteBatch.End();
        }
        
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = keyboard.GetCurrentState();

            fpsObject.Update(gameTime);
            playerObject.Update(gameTime);

            bool changeRoom = false;

            // Check if player has collided with wall
            if (roomObject.Intersects(playerObject.BoundingBox))
            {
                playerObject.Electrocuting = true;
            }

            if (playerObject.Electrocuting && playerObject.electrocutionFrame > 22)
            {
                playerObject.Electrocuting = false;
                playerObject.electrocutionFrame = 0;
                changeRoom = true;
                playerObject.MoveTo(new Vector2(30, 99));
            }

            if (playerObject.Position.Y == 0)
            {
                roomY--;
                changeRoom = true;
                playerObject.MoveTo(new Vector2(128, 184));
                roomObject.ClosedDoor = 'S';
            }
            if (playerObject.Position.X == 0)
            {
                roomX--;
                changeRoom = true;
                playerObject.MoveTo(new Vector2(230, 99));
                roomObject.ClosedDoor = 'E';
            }
            if (playerObject.Position.X == 256 - 8)
            {
                roomX++;
                changeRoom = true;
                playerObject.MoveTo(new Vector2(8, 93));
                roomObject.ClosedDoor = 'W';
            }
            if (playerObject.Position.Y == 192)
            {
                roomY++;
                changeRoom = true;
                playerObject.MoveTo(new Vector2(125, 5));
                roomObject.ClosedDoor = 'N';
            }            

            if (changeRoom)
            {
                GetMaze();
            }
        }
    }
}
