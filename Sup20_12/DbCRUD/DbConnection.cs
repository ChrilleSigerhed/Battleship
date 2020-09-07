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

        public static Player UpdateHighscoreListToDb(Player myPlayer)
        {
            return null;
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




CREATE TABLE public.highscore
(
    id integer NOT NULL,
    player_id integer NOT NULL,
    win boolean NOT NULL,
    date DATE NOT NULL DEFAULT CURRENT_DATE,
    "numberOfMoves" integer NOT NULL,
    player_id integer NOT NULL,
    PRIMARY KEY(id),
    CONSTRAINT player_id FOREIGN KEY(id)
        REFERENCES public.player(id) MATCH SIMPLE
        NOT VALID
)
WITH(
    OIDS = FALSE
);

ALTER TABLE public.highscore
    OWNER to sup_g12




CREATE TABLE public.highscore
(
    id serial PRIMARY KEY,
    win boolean NOT NULL,
    date DATE NOT NULL DEFAULT CURRENT_DATE,
    "numberOfMoves" integer NOT NULL,
    player_id integer NOT NULL,
    CONSTRAINT fk_player_id
    FOREIGN KEY(id)

    REFERENCES public.player(id)

    ON DELETE CASCADE
);