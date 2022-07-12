﻿using System;

namespace TestAutomatizationCenter.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
