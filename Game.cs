using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using static System.Formats.Asn1.AsnWriter;
using MySql.Data.MySqlClient;

namespace Space_Invader_Réparé
{
    public class Game
    {

        static DatabaseHandler dbHandler = new DatabaseHandler();


        /// <summary>
        /// The main player of the game.
        /// </summary>
        static Player player = new Player();

        /// <summary>
        /// List of all invaders present in the game.
        /// </summary>
        static List<Invader> invaders = new List<Invader>();

        /// <summary>
        /// List of all missiles shot by the player.
        /// </summary>
        static List<Missile> missiles = new List<Missile>();

        /// <summary>
        /// List of all missiles shot by the invaders.
        /// </summary>
        static List<InvaderMissile> invaderMissiles = new List<InvaderMissile>();

        /// <summary>
        /// Random number generator, used for random events like shooting.
        /// </summary>
        static Random random = new Random();

        /// <summary>
        /// The player's current score.
        /// </summary>
        static int score = 0;

        /// <summary>
        /// Counter to determine when invaders should move.
        /// </summary>
        private int moveCounter = 0;

        /// <summary>
        /// Frequency at which invaders should move.
        /// </summary>
        private int moveFrequency = 20;

        private string playerPseudo;

        /// <summary>
        /// Adds a missile to the game's missile list.
        /// </summary>
        /// <param name="missile">The missile to add.</param>
        public static void AddMissile(Missile missile)
        {
            missiles.Add(missile);
        }

        /// <summary>
        /// Begins the game loop, setting up and updating game entities.
        /// </summary>
        public void StartGame()
        {
            playerPseudo = GetPlayerPseudo();
            for (int i = 0; i < 10; i++)
            {
                invaders.Add(new Invader());
            }

            while (true)
            {
                Draw();
                player.HandleInput();
                UpdateMissiles();
                UpdateInvaderMissiles();
                CheckCollisions();
                InvadersShoot();
                Thread.Sleep(100);
            }
        }


        public void Draw()
        {
            Console.Clear();

            moveCounter++;
            if (moveCounter % moveFrequency == 0)
            {
                foreach (var invader in invaders)
                {
                    invader.Move(); // Ce déplacement n'aura lieu que tous les 5 cycles
                }
            }

            foreach (var invader in invaders)
            {
                invader.Draw();
            }

            foreach (var missile in missiles)
                missile.Draw();

            foreach (var invMissile in invaderMissiles)
                invMissile.Draw(); // Assurez-vous de dessiner les missiles des envahisseurs


            player.Draw();
            DrawScore(); // Appeler cette méthode à chaque fois que le cadre est redessiné
        }
        public void DrawScore()
        {
            Console.SetCursorPosition(Console.WindowWidth - 10, 0); // Positionne le curseur en haut à droite
            Console.Write("Score: " + score); // Écrire le score
        }

        public void UpdateMissiles()
        {
            for (int i = missiles.Count - 1; i >= 0; i--)
            {
                missiles[i].Move();

                if (missiles[i].PositionY < 0)
                    missiles.RemoveAt(i); // Remove missile if out of bounds
            }
        }
        public void CheckCollisions()
        {
            for (int i = missiles.Count - 1; i >= 0; i--)
            {
                for (int j = invaders.Count - 1; j >= 0; j--)
                {
                    if (missiles[i].PositionX == invaders[j].PositionX &&
                        missiles[i].PositionY == invaders[j].PositionY && // Utilisez PositionY ici, et non 0.
                        invaders[j].Health > 0)
                    {
                        // Collision détectée!

                        invaders[j].Health -= 1; // Réduit la santé de l'invader

                        if (invaders[j].Health <= 0)
                        {
                            score += 180; // Incrémente le score de 10 points si l'invader est détruit
                        }

                        missiles.RemoveAt(i); // Supprimer le missile du joueur après la collision
                        break; // Puisque ce missile est supprimé, passez au missile suivant
                    }
                }
            }

            // Après avoir géré les collisions de missiles avec les envahisseurs:
            bool allInvadersDestroyed = true; // Supposons que tous les envahisseurs sont détruits
            foreach (var invader in invaders)
            {
                if (invader.Health > 0)
                {
                    allInvadersDestroyed = false; // Un envahisseur est encore en vie
                    break;
                }
            }

            if (allInvadersDestroyed)
            {
                SaveScoreToDatabase(playerPseudo, score);
                Console.Clear();
                Console.WriteLine($"Bravo! Votre score est : {score}");
                Thread.Sleep(5000); // Attend 5 secondes pour que le joueur puisse voir le message
                Environment.Exit(0); // Termine le jeu, vous pourriez également initialiser le niveau suivant ici
            }
        }



        public void UpdateInvaderMissiles()
        {
            for (int i = invaderMissiles.Count - 1; i >= 0; i--)
            {
                invaderMissiles[i].Move();

                // Vérifiez si un missile d'envahisseur a atteint la position du joueur
                if (invaderMissiles[i].PositionY >= Console.WindowHeight - 1 && invaderMissiles[i].PositionX == player.Position)
                {
                    // Collision détectée avec le joueur
                    player.Health--; // Décrémente la santé du joueur
                    invaderMissiles.RemoveAt(i); // Supprime le missile

                    // Si la santé du joueur est zéro, terminez le jeu
                    if (player.Health <= 0)
                    {
                        SaveScoreToDatabase(playerPseudo, score);
                        Console.Clear();
                        Console.WriteLine("Game Over!");
                        

                        // Proposer de rejouer
                        Console.WriteLine("Voulez-vous rejouer? (y/n)");
                        char choix = Console.ReadKey().KeyChar;
                        if (choix == 'y' || choix == 'Y')
                        {
                            // Réinitialiser le jeu et le démarrer à nouveau.
                            // Ici, j'appelle simplement StartGame, mais vous pourriez vouloir réinitialiser certaines variables avant de le faire.
                            StartGame();
                        }
                        else
                        {
                            Environment.Exit(0); // Si le joueur ne veut pas rejouer, quittez le jeu.
                        }
                    }
                }
                else if (invaderMissiles[i].PositionY >= Console.WindowHeight)
                {
                    invaderMissiles.RemoveAt(i); // Supprime le missile s'il sort de l'écran
                }
            }
        }

        public void InvadersShoot()
        {
            foreach (var invader in invaders)
            {
                if (invader.Health > 0 && random.Next(100) < 2) // 2% chance for each living invader to shoot
                {
                    invaderMissiles.Add(new InvaderMissile(invader.PositionX, invader.PositionY + 1));
                }
            }
        }


   
        public static void ResetGame()
        {
            player = new Player();
            invaders.Clear();
            missiles.Clear();
            invaderMissiles.Clear();
            score = 0;

            for (int i = 0; i < 5; i++)
            {
                invaders.Add(new Invader());
            }
        }
        public static int GetMissileCount()
        {
            return missiles.Count;
        }

        public string GetPlayerPseudo()
        {
            Console.Clear();
            Console.WriteLine("Veuillez entrer votre pseudo:");
            string pseudo = Console.ReadLine();
            return pseudo;
        }
        public void SaveScoreToDatabase(string pseudo, int playerScore)
        {
            dbHandler.SavePlayerScore(pseudo, playerScore);
        }
    }
    


}

