using System;
using System.Collections.Generic;


namespace ZofimPortalApp.Models
{
    public class Shevet
    {
        public Shevet()
        {
            Cadets = new List<Cadet>();
            Parents = new List<Parent>();
            Workers = new List<Worker>();
        }

        public string Name { get; set; }
        public int HanhagaId { get; set; }
        public int MembersAmount { get; set; }
        public int Id { get; set; }

        public virtual Hanhaga Hanhaga { get; set; }
        public virtual List<Cadet> Cadets { get; set; }
        public virtual List<Parent> Parents { get; set; }
        public virtual List<Worker> Workers { get; set; }
    }
}
