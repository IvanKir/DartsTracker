using SQLite;

namespace DartsTracker.Models
{
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public int WinnerId { get; set; }
        public short Rounds { get; set; }
        public int GameType { get; set; }

        public Game(string groupName, string name, int gameType)
        {
            GroupName = groupName;
            Name = name;
            GameType = gameType;
        }

        public Game(){}
    }
}