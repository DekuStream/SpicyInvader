using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invader_Réparé
{
    public class Missile
    {

        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Missile(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }

        public void Move()
        {
            PositionY--; // Moves missile up
        }

        public void Draw()
        {
            Console.SetCursorPosition(PositionX, PositionY);
            Console.Write("|");
        }
    }
}
