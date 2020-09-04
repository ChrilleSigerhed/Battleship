using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Sup20_12.DbCRUD
{
    class DbConnection
    {


        private static string connectionString = ConfigurationManager.ConnectionStrings["dbLocal"].ConnectionString;
        //CRUD
        #region CREATE

        ///<summary>
        ///Skickar in ett objekt player med nickname men utan id och lägger till en spelare till databasen. Den returnerar sedan objektet player med id och nickname från databasen.
        ///</summary>
        public static Player AddPlayer(Player player)
        {
            string stmt = "INSERT INTO player (nickname) VALUES (@nickname) returning id";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    conn.Open();
                    command.Parameters.AddWithValue("nickname", player.Nickname);
                    int id = (int)command.ExecuteScalar();
                    player.Id = id;
                    return player;
                }
            }
        }
        #endregion

        #region READ
        ///<summary>
        ///Returnerar en lista på alla player objekt i databasen med nickname och id.
        ///</summary>
        public static IEnumerable<Player> GetPlayers()
        {
            string stmt = "SELECT id, nickname, id FROM player ORDER BY nickname;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                Player myPlayer = null;
                List<Player> myPlayerList = new List<Player>();
                conn.Open();

                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            myPlayer = new Player
                            {
                                Id = (int)reader["id"],
                                Nickname = (string)reader["firstname"],
                            };
                            myPlayerList.Add(myPlayer);
                        }
                    }
                }

                return myPlayerList;
            }
        }

        public static IEnumerable<Highscore> GetHighscore(int id)
        {
            string stmt = $"SELECT id, win, date, numberOfMoves FROM highscore WHERE playerId = {id} ORDER BY numberOfMoves DESC;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                Highscore myHighscore = null;
                List<Highscore> myHighscoreList = new List<Highscore>();
                conn.Open();

                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            myHighscore = new Highscore
                            {
                                Id = (int)reader["id"],
                                Win = (bool)reader["win"],
                                Date = (DateTime)reader["date"],
                                NumberOfMoves = (int)reader["numberOfmoves"]
                            };
                            myHighscoreList.Add(myHighscore);
                            command.Parameters.Clear();
                        }
                    }
                }
                return myHighscoreList;
            }
        }
        #endregion

        #region UPDATE
        private static void UpdateHighscore(Highscore myHighscore)
        {
                
        }
        #endregion

        #region DELETE
        private static void Delete()
        {
                
        }
        #endregion
    }
}
