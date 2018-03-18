using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

using ThesisDemoWebApi.Api;
using System.Net.Http; // HTTP stack library


namespace ThesisDemoWebApi.Hubs
{
    public class ChatHub : Hub
    {
        public readonly static ConnectionMapping<int> connections = new ConnectionMapping<int>();

        public void Send(string message)
        {
           

            Clients.Group((string)Clients.Caller.currentGroup).addMessage(Clients.Caller.userName, message, (string)Clients.Caller.currentGroup);
            //Clients.All.addMessage(Clients.Caller.userName, message);

        }

        public void JoinGroup(string groupID)
        {
            Groups.Add(Context.ConnectionId, groupID);
            Clients.Group(groupID).updateUsersInGroup(groupID);
            //Clients.Group(groupName).addMessage(Clients.Caller.userName + " joined.");
        }

        public void LeaveGroup(string groupID)
        {
            Groups.Remove(Context.ConnectionId, groupID);
            //Clients.Group(groupName).addMessage(Clients.Caller.userName + " joined.");
        }

        public override Task OnConnected()
        {
            //Context property returns a HubCallerContext
            var userID = int.Parse(Context.QueryString["UserID"]);
            connections.Add(userID, Context.ConnectionId);

            return base.OnConnected();
        }

        public async override Task OnDisconnected(bool stopCalled)
        {
            var userID = int.Parse(Context.QueryString["UserID"]);
            connections.Remove(userID, Context.ConnectionId);

            //Clients.Others.updateUsersInGroup(-1);

            var client = new HttpClient();
            var result = await client.GetAsync($"http://localhost:19216/api/User/{userID}");

            var user = await result.Content.ReadAsAsync<UserData>();

            foreach (var g in user.Groups)
            {
                Clients.Group(g.ID.ToString()).updateUsersInGroup(g.ID);
            };

            await base.OnDisconnected(stopCalled);
        }
    }
}