using System;
using System.Collections.Generic;


namespace ZofimPortalApp.Models
{
    public partial class CadetParent
    {
        public int ParentId { get; set; }
        public int CadetId { get; set; }

        public virtual Cadet Cadet { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
