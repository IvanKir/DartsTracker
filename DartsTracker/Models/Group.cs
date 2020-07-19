using SQLite;

namespace DartsTracker.Models
{
    public class Group
    {
        
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        public Group() { }

        public Group(string name )
        {
            Name = name;
        }
    }
}