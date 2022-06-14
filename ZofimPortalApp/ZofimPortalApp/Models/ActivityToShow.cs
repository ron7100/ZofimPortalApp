using System;
using System.Collections.Generic;
using System.Text;

namespace ZofimPortalApp.Models
{
    public class ActivityToShow
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RelevantClass { get; set; }
        /*
        0 - everyone
        1 - 4th, 5th grade
        2 - 6th, 7th, 8th grade
        3 - 10th, 11th, 12th grade
        4-12 - grade according to number
        */
        public int CadetsAmount { get; set; }
        public int Price { get; set; }
        public int? DiscountPercent { get; set; }
        public string IsOpen { get; set; }
        //Green - yes
        //Red - no
        public int ShevetID { get; set; }
        public string Shevet { get; set; }
        public string Hanhaga { get; set; }
        public int ID { get; set; }
    }
}
