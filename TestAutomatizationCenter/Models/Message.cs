namespace TestAutomatizationCenter.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public long TimeStamp { get; set; }
    }
}
