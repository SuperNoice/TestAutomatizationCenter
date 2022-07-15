using System.Collections.Generic;

namespace TestAutomatizationCenter.Models
{
    public class ChatContent
    {
        public List<string> Users { get; set; }
        public List<Message> Messages { get; set; }

        public ChatContent()
        {
            Users = new List<string>();
            Messages = new List<Message>();
        }
    }
}
