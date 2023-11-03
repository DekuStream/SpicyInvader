using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Space_Invader_Réparé
{
    public class Menu
    {
        static Game game = new Game();
        static DatabaseHandler SQL = new DatabaseHandler(); 
        public void Afficher()
        {
            bool continuer = true;

            while (continuer)
            {
                AfficherOptions();

                char choix = Console.ReadKey().KeyChar;

                switch (choix)
                {
                    case '1':
                        Jouer();
                        break;
                    case '2':
                        AfficherScore();
                        break;
                    case '3':
                        continuer = false;
                        break;
                    default:
                        Console.WriteLine("\nOption invalide. Veuillez choisir une option valide.");
                        break;
                }
            }
        }

        private void AfficherOptions()
        {
            Console.Clear();
            Console.WriteLine("------------ Spicy Invaders ------------");
            Console.WriteLine();
            Console.WriteLine("1. Jouer");
            Console.WriteLine("2. Score");
            Console.WriteLine("3. Quitter");
            Console.WriteLine();
            Console.Write("Entrez votre choix: ");
        }

        private void Jouer()
        {
            game.StartGame();

        }

        private void AfficherScore()
        {
           SQL.GetTopScores();
        }




       
    }

}

