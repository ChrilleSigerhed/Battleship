
namespace Sup20_12
{
   public class Player : IPlayer
    {
        public string Nickname { get; set; }
        public int Id { get; set; }
        public bool LastPlayer { get; set; }
        public long NumberOfGamesPlayed { get; set; }

        public Player(string nickname)
        {
            Nickname = nickname;
            LastPlayer = false;
        }
        public override string ToString()
        {
            return Nickname;
        }

    }
}
