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
        public bool Online { get; set; }
        public List<GroupData> Groups { get; set; }
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
                    Name = u.UserName,
                    Groups = u.Groups.Select(x => new GroupData { ID = x.ID, Name = x.GroupName }).ToList()
                };
            return result.ToArray();
        }

        public HttpResponseMessage Get(int id)
        {
            var user = context.Users.Include("Groups").SingleOrDefault(u => u.ID == id);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var userData = new UserData
            {
                ID = user.ID,
                Name = user.UserName,
                Groups = user.Groups.Select(x => new GroupData { ID = x.ID, Name = x.GroupName}).ToList()
            };

            return Request.CreateResponse(HttpStatusCode.OK, userData);
        }
    }
}
