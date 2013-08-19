using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace NBezerk
{
    public class RoomObject : GameObject
    {
        private WallObject topLeftWall = new WallObject(4, 0, 100, 4);
        private WallObject topRightWall = new WallObject(152, 0, 99, 4);

        private WallObject bottomLeftWall = new WallObject(4, 204, 100, 4);
        private WallObject bottomRightWall = new WallObject(152, 204, 99, 4);

        private WallObject leftTopWall = new WallObject(4, 0, 4, 72);
        private WallObject leftBottomWall = new WallObject(4, 136, 4, 72);

        private WallObject rightTopWall = new WallObject(248, 0, 4, 72);
        private WallObject rightBottomWall = new WallObject(248, 136, 4, 72);

		private WallObject[] mazeWalls = new WallObject[8];

		private string maze;

        public string Maze { get { return maze; } set { maze = value; UpdateMazeWalls(); } }

        public RoomObject()
        {
            for (int mazeWallIndex = 0; mazeWallIndex < mazeWalls.Length; mazeWallIndex++)
            {
                mazeWalls[mazeWallIndex] = new WallObject();
            }
        }

		public void UpdateMazeWalls()
        {
            for (int pillarIndex = 0; pillarIndex < 8; pillarIndex++)
            {
                DrawingPoint pillarLocation = new DrawingPoint(56 + (pillarIndex % 4) * 48, pillarIndex < 4 ? 68 : 136);

                char wallDirection = Maze[pillarIndex];

                mazeWalls[pillarIndex].Position.X = (wallDirection == 'W') ? pillarLocation.X - 48 : pillarLocation.X;
                mazeWalls[pillarIndex].Position.Y = (wallDirection == 'N') ? pillarLocation.Y - 68 : pillarLocation.Y;
                mazeWalls[pillarIndex].Size.X = (wallDirection == 'N' || wallDirection == 'S') ? 4 : 52;
                mazeWalls[pillarIndex].Size.Y = (wallDirection == 'N' || wallDirection == 'S') ? 72 : 4;

                // Walls drawn westwards don't include the starting pillar!
                if (wallDirection == 'W')
                {
                    mazeWalls[pillarIndex].Size.X -= 4;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            topLeftWall.Draw(spriteBatch);
            topRightWall.Draw(spriteBatch);

            bottomLeftWall.Draw(spriteBatch);
            bottomRightWall.Draw(spriteBatch);

            leftTopWall.Draw(spriteBatch);
            leftBottomWall.Draw(spriteBatch);

            rightTopWall.Draw(spriteBatch);
            rightBottomWall.Draw(spriteBatch);

            foreach (WallObject mazeWall in mazeWalls)
            {
                mazeWall.Draw(spriteBatch);
            }
        }
    }
}
