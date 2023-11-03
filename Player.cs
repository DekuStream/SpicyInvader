using Space_Invader_Réparé;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invader_Réparé
{
    public class Player
    {



        public int Position { get; set; } = 10;
        public int Health { get; set; } = 3; // Propriété Health

        public void Draw()
        {
            Console.SetCursorPosition(Position, Console.WindowHeight - 1);

            // Change la couleur du joueur en fonction de sa santé
            switch (Health)
            {
                case 3:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.Write("A");
            Console.ResetColor(); // Réinitialise la couleur pour les autres éléments dessinés à l'écran
        }

        public void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.LeftArrow && Position > 0)
                    Position--;
                else if (key.Key == ConsoleKey.RightArrow && Position < Console.WindowWidth - 1)
                    Position++;
                else if (key.Key == ConsoleKey.Spacebar)
                    Game.AddMissile(new Missile(Position, Console.WindowHeight - 2)); // Ajout du missile à travers une méthode
            }
        }



    }
}
