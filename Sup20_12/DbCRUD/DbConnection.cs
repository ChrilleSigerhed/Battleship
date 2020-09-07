using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Sup20_12
{
    class DbConnection
    {


        private static string connectionString = ConfigurationManager.ConnectionStrings["universitetet"].ConnectionString;
        //CRUD

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
        ///Skickar in ett objekt Highscore med id(från Player.Id), win, numberOfMoves men utan Date. Den returnerar sedan objektet Highscore med ett datum från Db.
        ///</summary>
        public static Highscore AddOneHighscoreToDb(Highscore myHighscore, int id)
        {
            string stmt = "INSERT INTO highscore (player_id, win, numberOfMoves) VALUES (@id, win, numberOfMoves) RETURNING date";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(stmt, conn))
                        {
                            command.Parameters.AddWithValue("player_id", id);
                            command.Parameters.AddWithValue("win", myHighscore.Win);
                            command.Parameters.AddWithValue("numberOfMoves", myHighscore.NumberOfMoves);
                            string date = (string)command.ExecuteScalar();
                            myHighscore.Date = date;
                        }
                        trans.Commit();
                        return myHighscore;
                    }
                    catch (PostgresException)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }




        #endregion

        #region READ
        ///<summary>
        ///Returnerar en List på alla player objekt i databasen med nickname, id och List med Highscore.
        ///</summary>
        ///

        public static IEnumerable<Player> GetAllPlayers() 
        {
            string stmt = "SELECT id, nickname FROM player ORDER BY nickname";

            using (var conn = new NpgsqlConnection(connectionString))
            {
               //Player myPlayer = null;
               List<Player> LstAllPlayers = new List<Player>();
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
                return LstAllPlayers;
            }
        }

    

        public static IEnumerable<Highscore> GetHighscore(int id) 
        {
            string stmt = $"SELECT id, win, date, number_of_moves FROM highscore WHERE player_id = {id} ORDER BY number_of_moves DESC;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
               // Highscore myHighscore = null;
                List<Highscore> LstHighscore = new List<Highscore>();
                conn.Open();

                using (var command = new NpgsqlCommand(stmt, conn))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Highscore myHighscore = new Highscore(0, true, 0)
                            {
                                Id = (int)reader["id"],
                                Win = (int)reader["win"],
                                Date = (DateTime)reader["date"],
                                NumberOfMoves = (int)reader["number_of_moves"],
                                PlayerId = (int)reader["player_id"]

                            };
                            LstHighscore.Add(myHighscore);
                        }
                    }
                }
                return LstHighscore;

            }

        }
        #endregion

        
        #region UPDATE

       
        #endregion

        
        #region DELETE
        
        #endregion
    }
}




