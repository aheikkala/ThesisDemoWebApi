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
    public class GroupData
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class GroupController : ApiController
    {
        private DataContext context;

        public GroupController()
        {
            context = new DataContext();

        }

        public HttpResponseMessage Get(int id)
        {
            var query =
                from g in context.Groups
                where g.ID == id
                select new GroupData
                {
                    ID = g.ID,
                    Name = g.GroupName
                };

            var group = query.SingleOrDefault();

            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, group);
        }

        // PUT /api/group/1
        // {name:testgroup}
        public HttpResponseMessage Put(int id, GroupData data)
        {
            var query =
                from g in context.Groups
                where g.ID == id
                select g;

            var group = query.SingleOrDefault();

            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            group.GroupName = data.Name;
            context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }


        [Route("user/{userId:int}/group")]
        [HttpPost]
        public HttpResponseMessage CreateGroup(int userID, GroupData data)
        {
            var group = new Group
            {
                GroupName = data.Name,
                CreationDate = DateTime.Now
            };

            var user = context.Users.Find(userID);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound); //parempi StatusCode?
            }

            context.Groups.Add(group);
            user.Groups.Add(group);

            context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
