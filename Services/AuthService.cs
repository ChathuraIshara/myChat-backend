using DataAccessLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
   
    public class AuthService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext dbcontext,IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            _configuration = configuration;
        }


        public User Register(User user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.password = hashedPassword;
            _dbcontext.users.Add(user);
            _dbcontext.SaveChanges();
            return user;
        }

            public string login(string username, string password)
            {
                User isExistUser = _dbcontext.users.FirstOrDefault(u => u.name == username);

                if (isExistUser == null)
                {
                    return null; // User not found
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, isExistUser.password);

                if (!isPasswordValid)
                {
                    return null; // Invalid password
                }

            string token = createToken(isExistUser);

                return token; // User authenticated successfully

        }

        public string createToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("Id",user.id.ToString()),
                new Claim("Name", user.name !),
                new Claim("Email",user.email !),
                new Claim("imgurl",user.userImgUrl !),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!.PadRight(64, '\0')));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(330).AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }


    }
}
