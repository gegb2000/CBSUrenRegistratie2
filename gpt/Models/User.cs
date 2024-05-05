using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // For simplicity, storing passwords in plain text. Consider hashing in production.
        public string Role { get; set; } // "Admin" or "Worker"
        public int RoleId { get; set; }
    }
}
