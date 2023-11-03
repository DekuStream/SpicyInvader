using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invader_Réparé
{
    public class InvaderMissile
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public InvaderMissile(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }

        public void Move()
        {
            PositionY++; // Moves missile down
        }

        public void Draw()
        {
            Console.SetCursorPosition(PositionX, PositionY);
            Console.Write("|");
        }
    }
}
