using ChatApp.DataService;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub:Hub
    {
        private readonly SharedDb _sharedDb;

        public ChatHub(SharedDb sharedDb)
        {
            _sharedDb = sharedDb;
            
        }

        public async Task joinChat(UserConnection conn)
        {
            await Clients.All.SendAsync("ReceiveMessage", "admin", $"{conn.userName} has joined");
        }

        public async Task joinSpecificChatRoom(UserConnection conn)
        {

            _sharedDb.connections[Context.ConnectionId]=conn;


            await Groups.AddToGroupAsync(Context.ConnectionId, conn.chatRoom);
            await Clients.Group(conn.chatRoom).SendAsync("ListenRoomJoining", "admin",$"{conn.userName} has joined {conn.chatRoom}");
        }

        public async Task sendMessage(String msg)
        {
            if(_sharedDb.connections.TryGetValue(Context.ConnectionId, out UserConnection conn)) {
                await Clients.Group(conn.chatRoom).SendAsync("ReceiveSpecificMessage",conn.userName,msg);
             }
        }



    }
}
