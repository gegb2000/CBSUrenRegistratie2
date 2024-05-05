using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSUrenRegistratie2.Models;

namespace CBSUrenRegistratie2.Services
{
    public class AuthService
    {
        private DatabaseService dbService = new DatabaseService();


        public async Task<User> Authenticate(string username, string password)
        {
            return await dbService.AuthenticateUser(username, password);
        }
    }
}
