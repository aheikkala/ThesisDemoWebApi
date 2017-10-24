using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThesisDemoWebApi.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public User User { get; set; }
    }
}