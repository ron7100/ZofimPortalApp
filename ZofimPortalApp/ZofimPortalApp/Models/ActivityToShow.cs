using System;
using System.Collections.Generic;
using System.Text;

namespace ZofimPortalApp.Models
{
    public class ActivityToShow
    {
        public string Name { get; set; }
        public Date StartDate { get; set; }
        public Date EndDate { get; set; }
        public string RelevantClass { get; set; }
        /*
        0 - everyone, כל השבט
        1 - 4th, 5th grade, צעירה
        2 - 6th, 7th, 8th grade, בוגרת
        3 - 10th, 11th, 12th grade, שכבג
        4-12 - grade according to number
        13 - פעילים
        */
        public int CadetsAmount { get; set; }
        public int Price { get; set; }
        public int? DiscountPercent { get; set; }
        public string IsOpen { get; set; }
        //Green - yes
        //Red - no
        public int ShevetID { get; set; }
        public string Shevet { get; set; }
        public int HanhagaID { get; set; }
        public string Hanhaga { get; set; }
        public int ID { get; set; }
    }
}
