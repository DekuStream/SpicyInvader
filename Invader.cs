using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invader_Réparé
{
    public class Invader
    {
        static Random random = new Random();
        public int PositionX { get; set; } = random.Next(0, Console.WindowWidth - 1); // Position horizontale
        public int PositionY { get; set; } = 0; // Position verticale initialement en haut de l'écran
        public int Health { get; set; } = 2; // Nouvelle propriété Health

        public void Move()
        {
            PositionY++; // Déplace l'envahisseur vers le bas

            // Si l'envahisseur atteint le bas de l'écran, réinitialisez sa position verticale à 0 et donnez-lui une nouvelle position horizontale aléatoire.
            if (PositionY >= Console.WindowHeight)
            {
                PositionY = 0;
                PositionX = random.Next(0, Console.WindowWidth - 1);
            }
        }

        public void Draw()
        {
            if (Health > 0) // Dessine l'envahisseur seulement s'il a de la santé
            {
                Console.SetCursorPosition(PositionX, PositionY);
                if (Health == 2)
                    Console.ForegroundColor = ConsoleColor.Green; // Définir la couleur du texte en vert
                else if (Health == 1)
                    Console.ForegroundColor = ConsoleColor.Red; // Définir la couleur du texte en rouge
                Console.Write("W");
                Console.ResetColor(); // Réinitialiser la couleur du texte à la valeur par défaut
            }
        }
    }
}
