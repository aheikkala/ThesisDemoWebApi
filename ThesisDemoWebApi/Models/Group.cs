using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThesisDemoWebApi.Models
{
    public class Group
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual List<User> Users { get; set; }
    }
}