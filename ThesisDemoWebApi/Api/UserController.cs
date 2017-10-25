using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using ThesisDemoWebApi.Models;
using ThesisDemoWebApi.Repository;

namespace ThesisDemoWebApi.Api
{
    // DTO for projecting entity (viewmodel)
    public class UserData
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class UserController : ApiController
    {
        private DataContext context;
        //private IRepository<User> userRepository = null;

        public UserController()
        {
            context = new DataContext();
            //this.userRepository = new Repository<User>();
        }

        // GET: api/User
        public IEnumerable<UserData> Get()
        {
            var result =
                from u in context.Users
                select new UserData
                {
                    ID = u.ID,
                    Name = u.UserName
                };
            return result.ToArray();
        }
    }
}
