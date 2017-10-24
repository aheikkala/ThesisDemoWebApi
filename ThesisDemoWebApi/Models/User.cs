using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThesisDemoWebApi.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public virtual List<Group> Groups { get; set; }
    }
}