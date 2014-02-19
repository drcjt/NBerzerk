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
    enum WallDirection
    {
        North,
        South,
        East,
        West
    };

    [Flags]
    public enum Cell
    {
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8
    }

    public class RoomObject : GameObject
    {
        private WallObject[] edgeWalls = new WallObject[8];
		private WallObject[,] mazeWalls = new WallObject[4,2];
        private WallObject doorWall = new WallObject(0, 0, 0, 0);

        // Represents the individual cells in the room
        // determining the walls adjacent to the cell
        Cell[,] cells = new Cell[5, 3];

        public Cell GetCell(Vector2 position)
        {
            return cells[((int)position.X - 10) / 48, (int)position.Y / 69];
        }

        public int GetCellIndex(Vector2 position)
        {
            return (((int)position.Y / 69) * 5) + (((int)position.X - 10) / 48);
        }

        private char closedDoor;
        public char ClosedDoor { get { return closedDoor; } set { closedDoor = value; UpdateDoorWall(); } }

        public RoomObject()
        {
            for (var column = 0; column < 4; column++)
            {
                for (var row = 0; row < 2; row++)
                {
                    mazeWalls[column, row] = new WallObject();
                }
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
            foreach (var edgeWall in edgeWalls)
            {
                result = result || edgeWall.BoundingBox.Intersects(value);
            }

            foreach (var mazeWall in mazeWalls)
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
                doorWall.MoveTo(5, 72);
                doorWall.Size = new Vector2(2, 64);
            }
            else if (closedDoor == 'E')
            {
                doorWall.MoveTo(249, 72);
                doorWall.Size = new Vector2(2, 64);
            }
            else if (closedDoor == 'N')
            {
                doorWall.MoveTo(104, 1);
                doorWall.Size = new Vector2(48, 2);
            }
            else if (closedDoor == 'S')
            {
                doorWall.MoveTo(104, 205);
                doorWall.Size = new Vector2(48, 2);
            }
            else
            {
                doorWall.MoveTo(0, 0);
                doorWall.Size = new Vector2(0, 0);
            }
        }

		private void UpdateMazeWalls(WallDirection[,] walls)
        {
            for (var column = 0; column < 4; column++)
            {
                for (var row = 0; row < 2; row++)
                {
                    var pillarLocation = new Point(56 + column * 48, 68 * (row + 1));
                    var wallDirection = walls[column, row];

                    int pillarPositionX = (wallDirection == WallDirection.West) ? pillarLocation.X - 48 : pillarLocation.X;
                    int pillarPositionY = (wallDirection == WallDirection.North) ? pillarLocation.Y - 68 : pillarLocation.Y;
                    mazeWalls[column, row].MoveTo(pillarPositionX, pillarPositionY);

                    Vector2 wallSize;
                    wallSize.X = (wallDirection == WallDirection.North || wallDirection == WallDirection.South) ? 4 : 52;
                    wallSize.Y = (wallDirection == WallDirection.North || wallDirection == WallDirection.South) ? 72 : 4;

                    // Walls drawn westwards don't include the starting pillar!
                    if (wallDirection == WallDirection.West)
                    {
                        wallSize.X -= 4;
                    }

                    mazeWalls[column, row].Size = wallSize;
                }
            }
        }

        public override void Draw(Screen screen)
        {
            foreach (var edgeWall in edgeWalls)
            {
                edgeWall.Draw(screen);
            }

            doorWall.Draw(screen);

            foreach (var mazeWall in mazeWalls)
            {
                mazeWall.Draw(screen);
            }
        }

        public void GenerateRoom(UInt16 roomNumber)
        {
            // Walls in each of screen cells, 5 wide by 3 high.
            // bit 1 = wall on left
            // bit 2 = wall on right
            // bit 3 = wall on top
            // bit 4 = wall on bottom.
            //int[,] walls = new int[5, 3];

            RandomNumberGenerator.seed = roomNumber;

            WallDirection[,] walls = new WallDirection[4, 2];

            Array.Clear(cells, 0, cells.Length);

            for (var row = 0; row < 2; row++)
            {
                for (var column = 0; column < 4; column++)
                {
                    RandomNumberGenerator.GetRandomNumber();
                    UInt16 pillarValue = RandomNumberGenerator.GetRandomNumber();
                    WallDirection wallDirection = GetNextWall(pillarValue);
                    walls[column, row] = wallDirection;

                    switch (wallDirection)
                    {
                        case WallDirection.North:
                            cells[column, row] |= Cell.Right;
                            cells[column + 1, row] |= Cell.Left;
                            break;

                        case WallDirection.South:
                            cells[column, row + 1] |= Cell.Right;
                            cells[column + 1, row + 1] |= Cell.Left;
                            break;

                        case WallDirection.East:
                            cells[column + 1, row] |= Cell.Bottom;
                            cells[column + 1, row + 1] |= Cell.Top;
                            break;

                        case WallDirection.West:
                            cells[column, row] |= Cell.Bottom;
                            cells[column, row + 1] |= Cell.Top;
                            break;
                    }
                }
            }

            for (var column = 0; column < 5; column++)
            {
                cells[column, 0] |= Cell.Top;
                cells[column, 2] |= Cell.Bottom;
            }

            for (var row = 0; row < 3; row++)
            {
                cells[0, row] |= Cell.Left;
                cells[4, row] |= Cell.Right;
            }

            UpdateMazeWalls(walls);
        }

        /// <summary>
        /// Convert the 16 bit number generated from the random number generator
        /// into a wall direction. This is done by taking the high 8 bits and
        /// looking at the 2 low bits of this.
        /// </summary>
        /// <param name="pillarValue">pillar value to get wall direction from</param>
        /// <returns>wall direction, i.e North, South, East, or West</returns>
        private WallDirection GetNextWall(UInt16 pillarValue)
        {
            var wallBits = (UInt16)(pillarValue & 3);
            return WallDirection.North + wallBits;
        }
    }
}