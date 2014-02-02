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
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace NBerzerk
{
    public class NBerzerkGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        static public KeyboardManager keyboardManager;

        private PrimitiveBatch<VertexPositionColor> batch;
        private SpriteBatch spriteBatch;
        private BasicEffect effect;

        static public Keyboard keyboard;

        private FPSObject fpsObject = new FPSObject();
        private HighScoresScreenObject highScoresScreenObject;
        private GamePlayObject gamePlayObject;

        private ComponentFramework.TextRendererObject textRendererObject = new ComponentFramework.TextRendererObject();

        bool contentLoaded = false;

        StateManager<ComponentFramework.GameObject> screenStateManager = new StateManager<ComponentFramework.GameObject>();

        Screen screen = new Screen(256, 224, 4, 4);

        public NBerzerkGame()
        {            
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            keyboardManager = new KeyboardManager(this);
            Content.Resolvers.Add(new EmbeddedResourceResolver());

            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            keyboard = new Keyboard(new DirectInput());
            keyboard.Acquire();

            highScoresScreenObject = new HighScoresScreenObject(screenStateManager);
            gamePlayObject = new GamePlayObject(screenStateManager);

            screenStateManager.AddState(highScoresScreenObject);
            screenStateManager.AddState(gamePlayObject);

            screenStateManager.CurrentState = highScoresScreenObject;
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
                fpsObject.LoadContent(Content);

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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var scaleMatrix = Matrix.Scaling(2.0f, 2.0f, 1.0f);
            spriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied, GraphicsDevice.SamplerStates.PointClamp, null, GraphicsDevice.RasterizerStates.CullNone, null, scaleMatrix);

            screen.Clear();
            screenStateManager.CurrentState.Draw(screen);
            screen.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            fpsObject.Draw(spriteBatch);
            spriteBatch.End();
        }
        
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = keyboardManager.GetState();

            fpsObject.Update(gameTime);
            screenStateManager.CurrentState.Update(gameTime);
        }
    }
}
