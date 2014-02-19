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

using NBerzerk.ComponentFramework;

namespace NBerzerk
{
    public class RobotObject : AnimatedObject
    {
        PlayerObject player;
        RoomObject room;

        public RobotObject(PlayerObject playerObject, RoomObject roomObject) : base("NBerzerk.Resources.robot.png")
        {
            player = playerObject;
            room = roomObject;

            Size = new Vector2(8, 11);
            Show = false;
            CurrentColor = new Color(108, 108, 0, 255);

            AddPattern("roving_eye", new int[] { 0, 1, 2, 3, 3, 3, 4, 5 });
            AddPattern("east", new int[] { 7, 6, 6 });
            AddPattern("south", new int[] { 0, 8, 0, 9 });
            AddPattern("west", new int[] { 10, 11, 11 });
            AddPattern("north", new int[] { 12, 13, 12, 14 });

            CurrentPattern = "roving_eye";

            AnimationSpeed = new TimeSpan(0, 0, 0, 0, 500);
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - LastAnimationTime > AnimationSpeed)
            {
                // Move towards play
                Vector2 delta = player.Position - this.Position;
                delta.X = Math.Sign(delta.X);
                delta.Y = Math.Sign(delta.Y);
                if (room.GetCellIndex(player.Position) == room.GetCellIndex(new Vector2(Position.X - 4, Position.Y - 4)))
                {
                    // If robot and man in same cell then robot always moves towards man
                    this.Move(delta.X, delta.Y);
                }
                else
                {
                    // Otherwise use IQ for robots to stop robots from walking into walls when going up and/or left
                    Cell robotCell1 = room.GetCell(new Vector2(Position.X - 4, Position.Y - 4));
                    Cell robotCell2 = room.GetCell(new Vector2(Position.X - 4 + 16, Position.Y - 4));
                    Cell robotCell3 = room.GetCell(new Vector2(Position.X - 4 + 16, Position.Y - 4 + 19));
                    Cell robotCell4 = room.GetCell(new Vector2(Position.X - 4, Position.Y - 4 + 19));

                    if (delta.X > 0 && ((robotCell1 | robotCell4) & Cell.Right) != 0)
                    {
                        delta.X = 0;
                    }
                    if (delta.X < 0 && ((robotCell2 | robotCell3) & Cell.Left) != 0)
                    {
                        delta.X = 0;
                    }
                    if (delta.Y > 0 && ((robotCell1 | robotCell2) & Cell.Bottom) != 0)
                    {
                        delta.Y = 0;
                    }
                    if (delta.Y < 0 && ((robotCell3 | robotCell4) & Cell.Top) != 0)
                    {
                        delta.Y = 0;
                    }
                    this.Move(delta.X, delta.Y);
                }

                if (delta.X == 0 && delta.Y == 0)   
                {   
                    CurrentPattern = "roving_eye";  
                }
                else if (delta.X > 0)
                { 
                    CurrentPattern = "east"; 
                }
                else if (delta.X < 0) 
                { 
                    CurrentPattern = "west"; 
                }
                else if (delta.Y < 0)                    
                {   
                    CurrentPattern = "north";       
                }
                else if (delta.Y > 0)                    
                {   
                    CurrentPattern = "south";       
                }
            }

            base.Update(gameTime);
        }
    }
}
