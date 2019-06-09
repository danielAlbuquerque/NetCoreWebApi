using System.Collections.Generic;
using Microsoft.Extensions.Options;
using TodoApi.Helpers;
using TodoApi.Models;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace TodoApi.Repositories
{
    public interface IUserRepository
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppSettings _appSettings;

        private List<User> _users = new List<User> 
        {
            new User { Id = 1, FirstName = "Daniel", LastName = "Albuquerque", UserName = "daniel", Password = "1234" }
        };

        public UserRepository(IOptions<AppSettings> AppSettings)
        {
            _appSettings = AppSettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.UserName == username && x.Password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _users.Select(x => {
                x.Password = null;
                return x;
            });
        }
    }
}