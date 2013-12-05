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

        private int robotFactor = 60;

        private int lives = 3;

        private RoomObject roomObject = new RoomObject();
        private PlayerObject playerObject = new PlayerObject();
        private RobotObject[] robotObjects =  new RobotObject[11];
        private FPSObject fpsObject = new FPSObject();
        private HighScoresScreenObject highScoresScreenObject = new HighScoresScreenObject();

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

            for (int robotIndex = 0; robotIndex < 11; robotIndex++)
            {
                robotObjects[robotIndex] = new RobotObject();
            }
        }

        /// <summary>
        /// Set the robot positions 
        /// </summary>
        private void SetRobotPositions()
        {
            Vector2[] robotStartPositions = { 
                new Vector2(206, 150),
                new Vector2(160, 150),
                new Vector2(64, 150),
                new Vector2(12, 150),
                new Vector2(158, 80),
                new Vector2(112, 80),
                new Vector2(64, 80),
                new Vector2(206, 12),
                new Vector2(160, 12),
                new Vector2(64, 12),
                new Vector2(12, 12)
            };

            // BCD used in randomizing robots - always starts at 60 for new game and cycles to
            // 20, 80, 40, 0 and then back to 60 whenever the player moves to new room or player dies
            robotFactor += 60;
            robotFactor = robotFactor % 100;

            IList<Vector2> robotPositions = new List<Vector2>();
            for (int robotNumber = 0; robotNumber < robotStartPositions.Length; robotNumber++)
            {
                UInt16 randomNumber = RandomNumberGenerator.GetRandomNumber();

                int convertedRobotFactor = int.Parse(robotFactor.ToString(), System.Globalization.NumberStyles.HexNumber);

                if (randomNumber >= convertedRobotFactor)
                {
                    Vector2 robotPosition = new Vector2();

                    randomNumber = RandomNumberGenerator.GetRandomNumber();
                    UInt16 x = (UInt16)robotStartPositions[robotNumber].X;
                    randomNumber = (UInt16)(randomNumber & 0x1F);
                    robotPosition.X = (UInt16)(x + randomNumber);

                    randomNumber = RandomNumberGenerator.GetRandomNumber();
                    UInt16 y = (UInt16)robotStartPositions[robotNumber].Y;
                    randomNumber = (UInt16)(randomNumber & 0x1F);
                    robotPosition.Y = (UInt16)(y + randomNumber);

                    Vector2 robotXY = new Vector2(robotPosition.X, robotPosition.Y);

                    robotObjects[robotNumber].MoveTo(robotXY);
                    robotObjects[robotNumber].Show = true;

                    robotPositions.Add(robotPosition);
                }
                else
                {
                    robotObjects[robotNumber].Show = false;
                }
            }
        }

        private void GetMaze()
        {
            UInt16 roomNo = (UInt16)((roomY << 8) + roomX);
            roomObject.Maze = MazeGenerator.GenerateMaze(roomNo);

            SetRobotPositions();
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

                foreach (RobotObject robotObject in robotObjects)
                {
                    robotObject.LoadContent(Content);
                }

                highScoresScreenObject.LoadContent(Content);
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
                    roomObject.Draw(spriteBatch);
                    playerObject.Draw(spriteBatch);

                    foreach (RobotObject robotObject in robotObjects)
                    {
                        robotObject.Draw(spriteBatch);
                    }
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
                        playerObject.MoveTo(new Vector2(30, 99));
                        GetMaze();
                        lives = 3;
                    }
                    break;

                case NBerzerkScreen.GamePlayScreen:
                    // Need to get new random number in here at a certain time interval - 1/60th of a second???
                    
                    playerObject.Update(gameTime);

                    bool changeRoom = false;

                    // Check if player has collided with wall
                    if (roomObject.Intersects(playerObject.BoundingBox) && !playerObject.Electrocuting)
                    {
                        playerObject.Electrocuting = true;
                        lives--;

                        if (lives == 0)
                        {
                            currentScreen = NBerzerkScreen.HighScoresScreen;
                            playerObject.Electrocuting = false;
                            break;
                        }
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
                        playerObject.MoveTo(new Vector2(223, 99));
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

                    break;

                case NBerzerkScreen.DemoScreen:
                    break;
            }
        }
    }
}
