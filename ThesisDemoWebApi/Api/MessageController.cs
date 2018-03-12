using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ThesisDemoWebApi.Hubs;
using ThesisDemoWebApi.Models;
using ThesisDemoWebApi.Repository;

namespace ThesisDemoWebApi.Api
{
    // DTO for projecting entity (viewmodel)
    public class MessageData
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int GroupID { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class MessageController : HubControllerBase<ChatHub>
    {
        private DataContext _context;

        public MessageController()
        {
            _context = new DataContext();
        }

        [Route("api/message/")]
        [HttpPost]
        public HttpResponseMessage CreateMessage(MessageData message)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState); // serializes ModelState that includes list of errors
            }

            var msg = new Message
            {
                Data = message.Message,
                Timestamp = DateTime.Now,
                User = _context.Users.Find(message.UserID)
            };

            var group = _context.Groups.Find(message.GroupID);

            _context.Messages.Add(msg);
            group.Messages.Add(msg);
            _context.SaveChanges();

            try
            {
                Hub.Clients.Group((string)message.GroupID.ToString()).addMessage(msg.User.UserName, message.Message, (string)message.GroupID.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return Request.CreateResponse(HttpStatusCode.NoContent); // success status code but does not return a body
        }

    }
}