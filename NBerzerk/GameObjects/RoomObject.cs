using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Toolkit.Graphics;

using NBerzerk.Common;
using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class RoomObject : GameObject
    {
        private WallObject[] edgeWalls = new WallObject[8];

		private WallObject[] mazeWalls = new WallObject[8];

        private WallObject doorWall = new WallObject(0, 0, 0, 0);

		private string maze;

        public string Maze { get { return maze; } set { maze = value; UpdateMazeWalls(); } }

        private char closedDoor;
        public char ClosedDoor { get { return closedDoor; } set { closedDoor = value; UpdateDoorWall(); } }

        public RoomObject()
        {
            for (int mazeWallIndex = 0; mazeWallIndex < mazeWalls.Length; mazeWallIndex++)
            {
                mazeWalls[mazeWallIndex] = new WallObject();
            }

            edgeWalls[0] = new WallObject(4, 0, 100, 4);
            edgeWalls[1] = new WallObject(152, 0, 99, 4);

            edgeWalls[2] = new WallObject(4, 204, 100, 4);
            edgeWalls[3] = new WallObject(152, 204, 99, 4);

            edgeWalls[4] = new WallObject(4, 0, 4, 72);
            edgeWalls[5] = new WallObject(4, 136, 4, 72);

            edgeWalls[6] = new WallObject(248, 0, 4, 72);
            edgeWalls[7] = new WallObject(248, 136, 4, 72);

            doorWall.Color = new Color(108, 108, 0, 255);
        }

        public bool Intersects(Rectangle value)
        {
            bool result = false;
            foreach (WallObject edgeWall in edgeWalls)
            {
                result = result || edgeWall.BoundingBox.Intersects(value);
            }

            foreach (WallObject mazeWall in mazeWalls)
            {
                result = result || mazeWall.BoundingBox.Intersects(value);
            }

            result = result || doorWall.BoundingBox.Intersects(value);

            return result;
        }

        private void UpdateDoorWall()
        {
            if (ClosedDoor == 'W')
            {
                doorWall.MoveTo(new Vector2(5, 72));
                doorWall.Size = new Vector2(2, 64);
            }
            else if (closedDoor == 'E')
            {
                doorWall.MoveTo(new Vector2(249, 72));
                doorWall.Size = new Vector2(2, 64);
            }
            else if (closedDoor == 'N')
            {
                doorWall.MoveTo(new Vector2(104, 1));
                doorWall.Size = new Vector2(48, 2);
            }
            else if (closedDoor == 'S')
            {
                doorWall.MoveTo(new Vector2(104, 205));
                doorWall.Size = new Vector2(48, 2);
            }
            else
            {
                doorWall.MoveTo(new Vector2(0, 0));
                doorWall.Size = new Vector2(0, 0);
            }
        }

		private void UpdateMazeWalls()
        {
            for (int pillarIndex = 0; pillarIndex < 8; pillarIndex++)
            {
                DrawingPoint pillarLocation = new DrawingPoint(56 + (pillarIndex % 4) * 48, pillarIndex < 4 ? 68 : 136);

                char wallDirection = Maze[pillarIndex];

                Vector2 pillarPosition;
                pillarPosition.X = (wallDirection == 'W') ? pillarLocation.X - 48 : pillarLocation.X;
                pillarPosition.Y = (wallDirection == 'N') ? pillarLocation.Y - 68 : pillarLocation.Y;
                mazeWalls[pillarIndex].MoveTo(pillarPosition);

                Vector2 wallSize;
                wallSize.X = (wallDirection == 'N' || wallDirection == 'S') ? 4 : 52;
                wallSize.Y = (wallDirection == 'N' || wallDirection == 'S') ? 72 : 4;

                // Walls drawn westwards don't include the starting pillar!
                if (wallDirection == 'W')
                {
                    wallSize.X -= 4;
                }

                mazeWalls[pillarIndex].Size = wallSize;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WallObject edgeWall in edgeWalls)
            {
                edgeWall.Draw(spriteBatch);
            }

            doorWall.Draw(spriteBatch);

            foreach (WallObject mazeWall in mazeWalls)
            {
                mazeWall.Draw(spriteBatch);
            }
        }
    }
}
