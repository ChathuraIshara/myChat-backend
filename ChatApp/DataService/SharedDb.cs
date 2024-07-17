using ChatApp.Models;
using System.Collections.Concurrent;

namespace ChatApp.DataService
{
    public class SharedDb
    {

        private readonly ConcurrentDictionary<string, UserConnection> _connections=new ();

        public ConcurrentDictionary<string, UserConnection> connections => _connections;
        private readonly ConcurrentDictionary<string, string> _userConnections = new(); // User ID to Connection ID mappin
        public ConcurrentDictionary<string, string> UserConnections => _userConnections; // Expose the new mapping



    }
}
