using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public class Role
    {
        public Role()
        {
            Cadets = new HashSet<Cadet>();
        }

        public string RoleName { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Cadet> Cadets { get; set; }
    }
}
