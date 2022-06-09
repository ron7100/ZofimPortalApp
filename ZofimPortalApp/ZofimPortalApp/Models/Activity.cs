using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public partial class Activity
    {
        public Activity()
        {
            ActivitiesHistories = new HashSet<ActivitiesHistory>();
        }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RelevantClass { get; set; }
        public int Price { get; set; }
        public int? DiscountPercent { get; set; }
        public int IsOpen { get; set; }
        public int ShevetId { get; set; }
        public int Id { get; set; }

        public virtual Shevet Shevet { get; set; }

        public virtual ICollection<ActivitiesHistory> ActivitiesHistories { get; set; }
    }
}
