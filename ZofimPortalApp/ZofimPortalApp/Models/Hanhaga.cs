using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public class Hanhaga
    {
        public Hanhaga()
        {
            Shevets = new List<Shevet>();
            Workers = new List<Worker>();
        }

        public string Name { get; set; }
        public int ShevetNumber { get; set; }
        public string GeneralArea { get; set; }
        public int Id { get; set; }

        public virtual List<Shevet> Shevets { get; set; }
        public virtual List<Worker> Workers { get; set; }
    }
}
