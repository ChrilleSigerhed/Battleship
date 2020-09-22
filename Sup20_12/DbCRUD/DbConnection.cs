using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;

namespace Sup20_12
{
    class DbConnection
    {


        private static string connectionString = ConfigurationManager.ConnectionStrings["universitetet"].ConnectionString;

        //CRUD
        public static void InitializeDbPooling()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            conn.Close();
        }

       
        //KLAR
        #region CREATE

        ///<summary>
        ///Skickar in ett objekt player med nickname men utan id och lägger till en spelare till databasen. Den returnerar sedan objektet Player med ett id från databasen.
        ///</summary>
        public static Player AddNewPlayerToDb(Player myPlayer)
        {
            string stmt = "INSERT INTO player (nickname) VALUES (@nickname) RETURNING id";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(stmt, conn))
                        {
                            command.Parameters.AddWithValue("nickname", myPlayer.Nickname);
                            int id = (int)command.ExecuteScalar();
                            myPlayer.Id = id;
                        }
                        trans.Commit();
                        return myPlayer;
                    }
                    catch (PostgresException)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        ///<summary>
        ///Skickar in ett objekt Highscore med id, playerId, win, numberOfMoves men utan Date och Id. Den returnerar sedan objektet Highscore med ett datum från Db.
        ///</summary>
        public static Highscore AddOneHighscoreToDb(Highscore myHighscore)
        {
            string stmt = "INSERT INTO highscore (win, number_of_moves, player_id) VALUES (@win, @number_of_moves, @player_id) RETURNING id";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(stmt, conn))
                        {
                            command.Parameters.AddWithValue("player_id", myHighscore.PlayerId);
                            command.Parameters.AddWithValue("win", myHighscore.Win);
                            command.Parameters.AddWithValue("number_of_moves", myHighscore.NumberOfMoves);
                            int id = (int)command.ExecuteScalar();
                            myHighscore.Id = id;
                        }
                        trans.Commit();
                       
                    }
                    catch (PostgresException)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return myHighscore = GetOneHighscoreById(myHighscore.Id);
        }




        #endregion

        #region READ

        ///<summary>
        ///Returnerar en List på alla player objekt i databasen med nickname, id och List med Highscore.
        ///</summary>
        public static ObservableCollection<Player> Players
        {
            get
            {
                string stmt = "SELECT id, nickname FROM player ORDER BY nickname";

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    //Player myPlayer = null;
                    ObservableCollection<Player> LstAllPlayers = new ObservableCollection<Player>();
                    conn.Open();

                    using (var command = new NpgsqlCommand(stmt, conn))
                    {

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player myPlayer = new Player("")
                                {
                                    Id = (int)reader["id"],
                                    Nickname = (string)reader["nickname"]

                                };
                                LstAllPlayers.Add(myPlayer);
                            }
                        }
                    }
                    conn.Close();
                    return LstAllPlayers;
                }
            }
        }

        private static Highscore GetOneHighscoreById(int id)
        {
            string stmt = $"SELECT id, win, date, number_of_moves, player_id FROM highscore WHERE id = {id};";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                Highscore myHighscore = null;
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            myHighscore = new Highscore(true, 0, 0)
                            {
                                Id = (int)reader["id"],
                                Win = (bool)reader["win"],
                                Date = (DateTime)reader["date"],
                                NumberOfMoves = (int)reader["number_of_moves"],
                                PlayerId = (int)reader["player_id"]
                            };
                        }
                    }
                }
                conn.Close();
                return myHighscore;
            }
        }

        public static IEnumerable<Highscore> GetThreeWinnersFromHighscore()
        {
            string stmt = $"SELECT highscore.id, win, date, number_of_moves, player_id, player.nickname FROM highscore INNER JOIN player on highscore.player_id = player.id WHERE win = true ORDER BY number_of_moves ASC, date DESC LIMIT 3";


            using (var conn = new NpgsqlConnection(connectionString))
            {
                //Highscore myHighscore = null;
                List<Highscore> LstAllHighscore = new List<Highscore>();
                conn.Open();

                using (var command = new NpgsqlCommand(stmt, conn))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Highscore allHighscore = new Highscore(true, 0, 0)
                            {
                                Id = (int)reader["id"],
                                Win = (bool)reader["win"],
                                Date = (DateTime)reader["date"],
                                NumberOfMoves = (int)reader["number_of_moves"],
                                PlayerId = (int)reader["player_id"],
                                Nickname = (string)reader["nickname"]
                            };
                            LstAllHighscore.Add(allHighscore);
                        }
                    }
                }
                conn.Close();
                return LstAllHighscore;
            }
        }

        public static IEnumerable<Player> GetThreeFrequentPlayersFromHighscore()
        {
            string stmt = $"SELECT player.nickname, COUNT(*) as games_played FROM player INNER JOIN highscore on player.id = highscore.player_id GROUP BY player.nickname, player.id ORDER BY COUNT(*) DESC LIMIT 3";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                List<Player> myThreeFrequentPlayers = new List<Player>();
                conn.Open();

                using (var command = new NpgsqlCommand(stmt, conn))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Player myPlayer = new Player("")
                            {
                                Nickname = (string)reader["nickname"],
                                NumberOfGamesPlayed = (long)reader["games_played"]
                            };
                            myThreeFrequentPlayers.Add(myPlayer);
                        }
                    }
                }
                conn.Close();
                return myThreeFrequentPlayers;
            }
        }

        public static bool IsPlayerNicknameUniqueInDb(string playerNickname)
        {
            bool result = false;
            string playerNicknameFromDb = "";
            playerNickname = playerNickname.ToUpper();
            string stmt = $"SELECT nickname FROM player WHERE UPPER (nickname) = '{playerNickname}'";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(stmt, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            {
                                playerNicknameFromDb = (string)reader["nickname"];
                            };
                        }
                    }
                }
                conn.Close();

                if (playerNicknameFromDb == "")
                    result = true;
                return result;
            }
        }
        #endregion


        #region UPDATE

        public static Player UpdateHighscoreListToDb(Player myPlayer)
        {
            return null;
        }
        #endregion

        
        #region DELETE
        private static void Delete()
        {
                
        }
        #endregion
    }
}
