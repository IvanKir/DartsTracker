using System.Collections.Generic;
using SQLite;

namespace DartsTracker.Models
{
    public class Player
    {

        public Player() { }
        
        public Player(string name, string groupName)
        {
            GroupName = groupName;
            Name = name;
        }
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public List<Game> Games = new List<Game>();
    }
}