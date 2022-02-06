using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public class Role
    {
        public Role()
        {
            Cadets = new List<Cadet>();
        }

        public string RoleName { get; set; }
        public int Id { get; set; }

        public virtual List<Cadet> Cadets { get; set; }
    }
}
