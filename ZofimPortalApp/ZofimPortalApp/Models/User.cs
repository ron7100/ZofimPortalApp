using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public partial class User
    {
        public User()
        {
            Cadets = new HashSet<Cadet>();
            Parents = new HashSet<Parent>();
            Workers = new HashSet<Worker>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Cadet> Cadets { get; set; }
        public virtual ICollection<Parent> Parents { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
