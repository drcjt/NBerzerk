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

        private FPSObject fpsObject = new FPSObject();
        private HighScoresScreenObject highScoresScreenObject = new HighScoresScreenObject();
        private GamePlayObject gamePlayObject = new GamePlayObject();

        private ComponentFramework.TextRendererObject textRendererObject = new ComponentFramework.TextRendererObject();

        bool contentLoaded = false;
        NBerzerkScreen currentScreen = NBerzerkScreen.HighScoresScreen;

        public NBerzerkGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.Resolvers.Add(new EmbeddedResourceResolver());

            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;

            keyboard = new Keyboard(new DirectInput());
            keyboard.Acquire();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (!contentLoaded)
            {
                batch = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);
                spriteBatch = new SpriteBatch(GraphicsDevice);
                effect = new BasicEffect(GraphicsDevice);

                highScoresScreenObject.LoadContent(Content);
                gamePlayObject.LoadContent(Content);
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix scaleMatrix = Matrix.Scaling(2.0f, 2.0f, 1.0f);
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied, GraphicsDevice.SamplerStates.PointClamp, null, GraphicsDevice.RasterizerStates.CullNone, null, scaleMatrix);

            switch (currentScreen)
            {
                case NBerzerkScreen.HighScoresScreen:
                    highScoresScreenObject.Draw(spriteBatch);
                    break;

                case NBerzerkScreen.GamePlayScreen:
                    gamePlayObject.Draw(spriteBatch);
                    break;

                case NBerzerkScreen.DemoScreen:
                    break;
            }

            spriteBatch.End();

            spriteBatch.Begin();
            fpsObject.Draw(spriteBatch);
            spriteBatch.End();
        }
        
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = keyboard.GetCurrentState();

            fpsObject.Update(gameTime);

            switch (currentScreen)
            {
                case NBerzerkScreen.HighScoresScreen:
                    if (highScoresScreenObject.Update(gameTime))
                    {
                        currentScreen = NBerzerkScreen.GamePlayScreen;
                        gamePlayObject.StartGame();
                    }
                    break;

                case NBerzerkScreen.GamePlayScreen:
                    if (gamePlayObject.Update(gameTime))
                    {
                        currentScreen = NBerzerkScreen.HighScoresScreen;
                    }
                    break;

                case NBerzerkScreen.DemoScreen:
                    break;
            }
        }
    }
}
