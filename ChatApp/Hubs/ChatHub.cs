using ChatApp.DataService;
using ChatApp.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub:Hub
    {
        private readonly SharedDb _sharedDb;
        private readonly ApplicationDbContext _context;

        public ChatHub(SharedDb sharedDb,ApplicationDbContext dbcontext)
        {
            _sharedDb = sharedDb;
            _context = dbcontext;
            
        }

        public async Task joinChat(UserConnection conn)
        {
            await Clients.All.SendAsync("ReceiveMessage", "admin", $"{conn.userId} has joined");
        }

        public async Task joinSpecificChatRoom(UserConnection conn)
        {

            _sharedDb.connections[Context.ConnectionId]=conn;


            await Groups.AddToGroupAsync(Context.ConnectionId, conn.chatRoom);
            await Clients.Group(conn.chatRoom).SendAsync("ListenRoomJoining", "admin",$"{conn.userId} has joined {conn.chatRoom}");
        }

        public async Task sendMessage(String msg)
        {
            if(_sharedDb.connections.TryGetValue(Context.ConnectionId, out UserConnection conn)) {
                await Clients.Group(conn.chatRoom).SendAsync("ReceiveSpecificMessage",conn.userId,msg);
             }
        }



    }
}
