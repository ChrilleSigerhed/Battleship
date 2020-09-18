using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12
{
    static class TestClass
    {
        public static void TestWritePlayer()
        {
            Player myPlayer = new Player("MrNoName");
            myPlayer = DbConnection.AddNewPlayerToDb(myPlayer);

            Highscore myHighscore = new Highscore(true, 20, myPlayer.Id);
            myHighscore = DbConnection.AddOneHighscoreToDb(myHighscore);

        }
    }
}
