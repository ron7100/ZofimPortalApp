using System;
using System.Collections.Generic;
using System.Text;

namespace ZofimPortalApp.Models
{
    class ParentToShow
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalID { get; set; }
        public string Shevet { get; set; }
        public string Hanhaga { get; set; }
        public int KidsNumber { get; set; }
    }
}
