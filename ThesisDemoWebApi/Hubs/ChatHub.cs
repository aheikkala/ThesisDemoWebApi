using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;


namespace ThesisDemoWebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public void Send(string message)
        {
           

            Clients.Group((string)Clients.Caller.currentGroup).addMessage(Clients.Caller.userName, message, (string)Clients.Caller.currentGroup);
            //Clients.All.addMessage(Clients.Caller.userName, message);

        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
            //Clients.Group(groupName).addMessage(Clients.Caller.userName + " joined.");
        }

        public void LeaveGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
            //Clients.Group(groupName).addMessage(Clients.Caller.userName + " joined.");
        }

        public override Task OnConnected()
        {
            //Context property returns a HubCallerContext
            string name = Context.QueryString["UserName"];
            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }
        //public override Task OnDisconnected()
        //{
        //    string name = Context.QueryString["UserName"];
        //    _connections.Remove(name, Context.ConnectionId);

        //    return base.OnDisconnected();
        //}
    }
}