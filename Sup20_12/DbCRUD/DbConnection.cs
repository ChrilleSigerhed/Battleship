using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Sup20_12.DbCRUD
{
    class DbConnection
    {


        private static string connectionString = ConfigurationManager.ConnectionStrings["universitetet"].ConnectionString;
        //CRUD

        //Klar för test LÄGG TILL TRY CATCH
        #region CREATE

        ///<summary>
        ///Skickar in ett objekt player med nickname men utan id och lägger till en spelare till databasen. Den returnerar sedan objektet player med id och nickname från databasen.
        ///</summary>
        public static Player AddPlayer(Player myPlayer)
        {
            string stmt = "INSERT INTO player (nickname) VALUES (@nickname) RETURNING id";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction()) //För att HELA transaktionen ska köras och godkännas (allt eller inget)
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(stmt, conn))
                        {
                            command.Parameters.AddWithValue("nickname", myPlayer.Nickname);
                            int id = (int)command.ExecuteScalar();
                            myPlayer.Id = id;
                        }
                        trans.Commit(); //När HELA transaktionen har godkänts så ska den kommittas och gå in i vår db
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


        #endregion

        #region READ
        ///<summary>
        ///Returnerar en lista på alla player objekt i databasen med nickname och id.
        ///</summary>
        public static IEnumerable<Player> GetPlayers() //Byt private mot public när denna funktion är klar
        {
            return null;
        }

        private static IEnumerable<Highscore> GetHighscore(int id) //Byt private mot public när denna funktion är klar
        {
            string stmt = $"SELECT id, win, date, numberOfMoves FROM highscore WHERE playerId = {id} ORDER BY numberOfMoves DESC;";
            return null;
        }
        #endregion

        //Inte klar
        #region UPDATE
        private static void UpdateHighscore(Highscore myHighscore)
        {
                
        }
        #endregion

        //Inte klar, ska nog inte vara med heller...
        #region DELETE
        private static void Delete()
        {
                
        }
        #endregion
    }
}
