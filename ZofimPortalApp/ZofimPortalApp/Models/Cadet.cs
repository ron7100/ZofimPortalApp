using System;
using System.Collections.Generic;


namespace ZofimPortalApp.Models
{
    public partial class Cadet
    {
        public Cadet()
        {
            ActivitiesHistories = new HashSet<ActivitiesHistory>();
        }

        public string FName { get; set; }
        public string LName { get; set; }
        public string PersonalId { get; set; }
        public int ShevetId { get; set; }
        public int RoleId { get; set; }
        public int Id { get; set; }

        public virtual Role Role { get; set; }
        public virtual Shevet Shevet { get; set; }
        public virtual ICollection<ActivitiesHistory> ActivitiesHistories { get; set; }
    }
}
