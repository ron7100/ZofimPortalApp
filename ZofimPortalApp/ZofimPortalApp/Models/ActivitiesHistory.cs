using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public class ActivitiesHistory
    {
        public int CadetId { get; set; }
        public string Activity { get; set; }

        public virtual Cadet Cadet { get; set; }
    }
}
