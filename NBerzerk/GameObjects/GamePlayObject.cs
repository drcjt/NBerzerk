using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.DirectInput;

using NBerzerk.Common;
using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class GamePlayObject : GameObject
    {
        private RoomObject roomObject = new RoomObject();
        private PlayerObject playerObject = new PlayerObject();
        private RobotObject[] robotObjects = new RobotObject[11];

        private int robotFactor = 60;

        private UInt16 roomY = 49;
        private UInt16 roomX = 83;

        private int lives = 3;

        bool contentLoaded = false;

        private StateManager<GameObject> stateManager;

        public GamePlayObject(StateManager<GameObject> stateManager)
        {
            this.stateManager = stateManager;

            for (var robotIndex = 0; robotIndex < 11; robotIndex++)
            {
                robotObjects[robotIndex] = new RobotObject(playerObject, roomObject);
            }
        }

        public override void EnterState()
        {
            playerObject.MoveTo(30, 99);
            playerObject.Electrocuting = false;
            GetMaze();
            lives = 3;
        }

        public override void LoadContent(IContentManager mgr)
        {
            base.LoadContent(mgr);

            if (!contentLoaded)
            {
                playerObject.LoadContent(mgr);

                foreach (var robotObject in robotObjects)
                {
                    robotObject.LoadContent(mgr);
                }

                contentLoaded = true;
            }
        }

        public override void Draw(Screen screen)
        {
            roomObject.Draw(screen);

            foreach (var robotObject in robotObjects)
            {
                robotObject.Draw(screen);
            }

            playerObject.Draw(screen);
        }

        public override void Update(GameTime gameTime)
        {
            // Need to get new random number in here at a certain time interval - 1/60th of a second???

            playerObject.Update(gameTime);

            foreach (var robotObject in robotObjects)
            {
                if (robotObject.Show)
                {
                    robotObject.Update(gameTime);
                }
            }

            // Check if player has collided with wall
            if (roomObject.Intersects(playerObject.BoundingBox) && !playerObject.Electrocuting)
            {
                playerObject.Electrocuting = true;
                lives--;

            }

            if (playerObject.Electrocuting && playerObject.PatternFrameIndex == 21)
            {
                if (lives == 0)
                {
                    stateManager.SwitchState(typeof(HighScoresScreenObject).Name);
                }
                else
                {
                    playerObject.Electrocuting = false;
                    playerObject.MoveTo(30, 99);
                    GetMaze();
                }
            }

            if (playerObject.Position.Y == 0)
            {
                roomY--;
                playerObject.MoveTo(null, 184);
                roomObject.ClosedDoor = 'S';
                GetMaze();
            }
            if (playerObject.Position.X == 0)
            {
                roomX--;
                playerObject.MoveTo(223, null);
                roomObject.ClosedDoor = 'E';
                GetMaze();
            }
            if (playerObject.Position.X == 256 - 8)
            {
                roomX++;
                playerObject.MoveTo(8, null);
                roomObject.ClosedDoor = 'W';
                GetMaze();
            }
            if (playerObject.Position.Y == 192)
            {
                roomY++;
                playerObject.MoveTo(null, 5);
                roomObject.ClosedDoor = 'N';
                GetMaze();
            }
        }

        private void GetMaze()
        {
            roomObject.GenerateRoom((UInt16)((roomY << 8) + roomX));
            SetRobotPositions();
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

            var robotPositions = new List<Vector2>();
            for (var robotNumber = 0; robotNumber < robotStartPositions.Length; robotNumber++)
            {
                var randomNumber = RandomNumberGenerator.GetRandomNumber();

                var convertedRobotFactor = int.Parse(robotFactor.ToString(), System.Globalization.NumberStyles.HexNumber);

                if (randomNumber >= convertedRobotFactor)
                {
                    randomNumber = RandomNumberGenerator.GetRandomNumber();
                    var x = (UInt16)robotStartPositions[robotNumber].X;
                    randomNumber = (UInt16)(randomNumber & 0x1F);
                    var robotPositionX = (UInt16)(x + randomNumber);

                    randomNumber = RandomNumberGenerator.GetRandomNumber();
                    var y = (UInt16)robotStartPositions[robotNumber].Y;
                    randomNumber = (UInt16)(randomNumber & 0x1F);
                    var robotPositionY = (UInt16)(y + randomNumber);

                    robotObjects[robotNumber].MoveTo(robotPositionX, robotPositionY);
                    robotObjects[robotNumber].Show = true;
                }
                else
                {
                    robotObjects[robotNumber].Show = false;
                }
            }
        }
    }
}
