using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
   
    public class UserService
    {
        private readonly ApplicationDbContext _dbcontext;
        public UserService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task <List<User>> GetUsers()
        {
            var users = await _dbcontext.users.ToListAsync();
            return users;
        }
        public async Task<User> updateUser(int id,User user)
        {
            var target=_dbcontext.users.FirstOrDefault(x => x.id == id);
            target.name=user.name;
            target.email = user.email;
            target.userImgUrl = user.userImgUrl;
            _dbcontext.SaveChanges();
            return target;
        }
    }
}
