namespace DartsTracker.Models
{
    public class Item
    {
        public string Type { get; set; }
        public int Value { get; set; }

        public Item(string type, int value)
        {
            Type = type;
            Value = value;
        }
    }
}