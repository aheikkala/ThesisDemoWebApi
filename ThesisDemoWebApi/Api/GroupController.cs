using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using ThesisDemoWebApi.Models;
using ThesisDemoWebApi.Repository;
using ThesisDemoWebApi.Hubs;

namespace ThesisDemoWebApi.Api
{
    // DTO for projecting database entity (viewmodel)
    public class GroupData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<UserData> Users { get; set; }
        public List<MessageData> Messages { get; set; }
    }

    public class GroupController : HubControllerBase<ChatHub>
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
                    Name = g.GroupName,
                    Users = g.Users.Select(x => new UserData { ID = x.ID, Name = x.UserName }).ToList(),
                    Messages = g.Messages.Select(x => new MessageData { UserName = x.User.UserName, Message = x.Data, TimeStamp = x.Timestamp }).ToList()
                };

            var group = query.SingleOrDefault();

            foreach (var u in group.Users)
            {
                if (ChatHub.connections.GetConnections(u.ID).Count() > 0)
                {
                    u.Online = true;
                }
            }


            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, group);
        }

        // PUT /api/group/1
        // {name:testgroup}
        public HttpResponseMessage Put(GroupData data) 
        {
            var query =
                from g in context.Groups
                where g.ID == data.ID
                select g;

            var group = query.SingleOrDefault();

            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            //users.Where(x => !x.Groups.Exists(g=> g.ID == _iGroupID)).ToList()
            //var users = context.Users.Where(u => data.Users.Select(o => o.ID).ToList().Contains(u.ID)).ToList();

            var userToAdd = data.Users[0].ID;

            var user = context.Users.SingleOrDefault(u => u.ID == userToAdd);

            //group.GroupName = data.Name;
            group.Users.Add(user);
            context.SaveChanges();

            // Notify added user about new group.
            foreach (var conn in ChatHub.connections.GetConnections(user.ID))
            {
                Hub.Clients.Client(conn).updateGroups();
            };

            // Notify users already in group about new user.
            Hub.Clients.Group(data.ID.ToString()).updateUsersInGroup(data.ID);

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

        //[Route("api/user/{userID:int}/group")]
        //[HttpGet]
        //public HttpResponseMessage GetGroupByUser(int userID)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState); // serializes ModelState that includes list of errors
        //    }

        //    var user = context.Users.Include("Groups").SingleOrDefault(u => u.ID == userID);

        //    if (user == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest); // client error
        //        //return Request.CreateResponse(HttpStatusCode.BadRequest, new { errors = new string[] {"Bad request."} });
        //    }

        //    context.Groups.Add(group);
        //    user.Groups.Add(group);

        //    context.SaveChanges();

        //    return Request.CreateResponse(HttpStatusCode.NoContent); // success status code but does not return a body
        //}
    }
}
