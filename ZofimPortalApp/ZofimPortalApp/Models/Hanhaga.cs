using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public partial class Hanhaga
    {
        public Hanhaga()
        {
            Shevets = new HashSet<Shevet>();
            Workers = new HashSet<Worker>();
        }

        public string Name { get; set; }
        public int ShevetNumber { get; set; }
        public string GeneralArea { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Shevet> Shevets { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
