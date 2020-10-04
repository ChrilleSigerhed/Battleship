namespace Sup20_12
{
    internal interface IPlayer
    {
        string Nickname { get; set; }
        public int Id { get; set; }
        public long NumberOfGamesPlayed { get; set; }
    }
}
