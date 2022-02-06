using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public class User
    {
        public User()
        {
            Parents = new List<Parent>();
            Workers = new List<Worker>();
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }

        public virtual List<Parent> Parents { get; set; }
        public virtual List<Worker> Workers { get; set; }
    }
}
