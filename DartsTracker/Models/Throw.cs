using SQLite;

namespace DartsTracker.Models
{
    public class Throw
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PlayerId { get; set;}
        public int GameId { get; set; }
        public byte First { get; set; }
        public byte Second { get; set; }
        public byte Third { get; set; }

        public Throw() {}

        public Throw(int playerId, int gameId, byte first, byte second, byte third)
        {
            PlayerId = playerId;
            GameId = gameId;
            First = first;
            Second = second;
            Third = third;
        }
    }
}