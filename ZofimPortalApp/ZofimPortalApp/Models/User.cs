using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public class User
    {
        public User()
        {
            Parents = new HashSet<Parent>();
            Workers = new HashSet<Worker>();
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Parent> Parents { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
