using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Space_Invader_Réparé
{
    internal class DatabaseHandler
    {
        private string connectionString;

        public DatabaseHandler()
        {
            connectionString = "server=localhost;user=root;database=db_space_invaders;port=6033;password=root";
        }

        public void GetTopScores()
        {
            Console.Clear();
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT jouPseudo, jouNombrePoints FROM t_joueur ORDER BY jouNombrePoints DESC LIMIT 10";
            MySqlCommand command = new MySqlCommand(query, connection);

            using MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string pseudo = reader["jouPseudo"].ToString();
                int score = Convert.ToInt32(reader["jouNombrePoints"]);

                Console.WriteLine($"Pseudo: {pseudo}, Score: {score}");
            }

            Console.WriteLine("\nAppuyer sur 'Entrée' pour continuer");
            Console.ReadLine();
            Console.Clear();

            connection.Close();
        }

        public void SavePlayerScore(string pseudo, int score)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "INSERT INTO t_joueur (jouPseudo, jouNombrePoints) VALUES (@pseudo, @score)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@pseudo", pseudo);
            command.Parameters.AddWithValue("@score", score);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
