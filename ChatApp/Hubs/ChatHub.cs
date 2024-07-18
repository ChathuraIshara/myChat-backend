using ChatApp.DataService;
using ChatApp.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace ChatApp.Hubs
{
    public class ChatHub:Hub
    {
        private readonly SharedDb _sharedDb;
        private readonly ApplicationDbContext _dbcontext;

        public ChatHub(SharedDb sharedDb,ApplicationDbContext dbcontext)
        {
            _sharedDb = sharedDb;
            _dbcontext = dbcontext;
            
        }

        public async Task joinChat(UserConnection conn)
        {
            await Clients.All.SendAsync("ReceiveMessage", "admin", $"{conn.userId} has joined");
        }

        public async Task joinSpecificChatRoom(UserConnection conn)
        {

            _sharedDb.connections[Context.ConnectionId]=conn;


            await Groups.AddToGroupAsync(Context.ConnectionId, conn.chatRoom);
            User targetUser = null;

            if (conn != null)
            {
                targetUser = _dbcontext.users.FirstOrDefault(x => x.id.ToString() == conn.userId);
            }
            await Clients.Group(conn.chatRoom).SendAsync("ListenRoomJoining", "admin",$"{targetUser.name} has joined {conn.chatRoom}");
        }

        public async Task sendMessage(String msg)
        {
            if(_sharedDb.connections.TryGetValue(Context.ConnectionId, out UserConnection conn)) {

                User targetUser = null;
               
                if (conn!=null)
                {
                   targetUser = _dbcontext.users.FirstOrDefault(x => x.id.ToString() == conn.userId);
                }
                await Clients.Group(conn.chatRoom).SendAsync("ReceiveSpecificMessage",targetUser,msg);
             }
        }



    }
}
