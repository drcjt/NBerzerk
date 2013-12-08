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

        public GamePlayObject()
        {
            for (int robotIndex = 0; robotIndex < 11; robotIndex++)
            {
                robotObjects[robotIndex] = new RobotObject();
            }
        }

        public void StartGame()
        {
            playerObject.MoveTo(new Vector2(30, 99));
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

                foreach (RobotObject robotObject in robotObjects)
                {
                    robotObject.LoadContent(mgr);
                }

                contentLoaded = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            roomObject.Draw(spriteBatch);
            playerObject.Draw(spriteBatch);

            foreach (RobotObject robotObject in robotObjects)
            {
                robotObject.Draw(spriteBatch);
            }
        }

        public new bool Update(GameTime gameTime)
        {
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
                    return true;
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

            return false;
        }

        private void GetMaze()
        {
            UInt16 roomNo = (UInt16)((roomY << 8) + roomX);
            roomObject.Maze = MazeGenerator.GenerateMaze(roomNo);

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
    }
}
