using System;
using System.Collections.Generic;
using System.Text;

namespace Sup20_12
{
    static class TestClass
    {
        public static void TestWrite()
        {
            Player myPlayer = new Player(("Kalle"));
            DbConnection.AddPlayer(myPlayer);
        }
    }
}
