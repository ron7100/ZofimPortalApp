using System;
using System.Collections.Generic;


namespace ZofimPortalApp.Models
{
    public class Cadet
    {
        public Cadet()
        {
            ActivitiesHistories = new List<ActivitiesHistory>();
        }

        public string FName { get; set; }
        public string LName { get; set; }
        public string Class { get; set; }
        public string PersonalId { get; set; }
        public int ShevetId { get; set; }
        public int RoleId { get; set; }
        public int Id { get; set; }

        public virtual Role Role { get; set; }
        public virtual Shevet Shevet { get; set; }
        public virtual List<ActivitiesHistory> ActivitiesHistories { get; set; }
    }
}
