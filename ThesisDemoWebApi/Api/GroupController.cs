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
    // DTO for projecting database entity (viewmodel)
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


        [Route("api/user/{userID:int}/group")]
        [HttpPost]
        public HttpResponseMessage CreateGroup(int userID, GroupData data)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState); // serializes ModelState that includes list of errors
            }

            var group = new Group
            {
                GroupName = data.Name,
                CreationDate = DateTime.Now
            };

            //var user = context.Users.Find(userID);
            var user = context.Users.Include("Groups").SingleOrDefault(x => x.ID == userID);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest); // client error
                //return Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = new string[] {"Bad request."} });
            }

            context.Groups.Add(group);
            user.Groups.Add(group);

            context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.NoContent); // success status code but does not return a body
        }
    }
}
