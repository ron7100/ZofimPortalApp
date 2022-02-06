using System;
using System.Collections.Generic;


namespace ZofimPortalApp.Models
{
    public class Shevet
    {
        public Shevet()
        {
            Cadets = new HashSet<Cadet>();
            Parents = new HashSet<Parent>();
            Workers = new HashSet<Worker>();
        }

        public string Name { get; set; }
        public int HanhagaId { get; set; }
        public int MembersAmount { get; set; }
        public int Id { get; set; }

        public virtual Hanhaga Hanhaga { get; set; }
        public virtual ICollection<Cadet> Cadets { get; set; }
        public virtual ICollection<Parent> Parents { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
