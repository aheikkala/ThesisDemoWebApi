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
        public List<Group> Groups { get; set; }
    }
}