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
        //מיסי חבר - הרשמה לשבט
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RelevantClass { get; set; }
        /*
        0 - everyone
        1 - 4th, 5th grade
        2 - 6th, 7th, 8th grade
        3 - 10th, 11th, 12th grade
        4-12 - grade according to number
        13 - פעילים
        */
        public int CadetsAmount { get; set; }
        public int Price { get; set; }
        public int? DiscountPercent { get; set; }
        public int IsOpen { get; set; }
        //1 - yes
        //0 - no
        public int ShevetId { get; set; }
        public int HanhagaId { get; set; }
        public int Id { get; set; }

        public virtual Hanhaga Hanhaga { get; set; }

        public virtual Shevet Shevet { get; set; }

        public virtual ICollection<ActivitiesHistory> ActivitiesHistories { get; set; }
    }
}
