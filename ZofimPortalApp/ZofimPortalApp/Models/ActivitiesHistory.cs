using System;
using System.Collections.Generic;

#nullable disable

namespace ZofimPortalServerBL.Models
{
    public partial class ActivitiesHistory
    {
        public int CadetId { get; set; }
        public string Activity { get; set; }

        public virtual Cadet Cadet { get; set; }
    }
}
