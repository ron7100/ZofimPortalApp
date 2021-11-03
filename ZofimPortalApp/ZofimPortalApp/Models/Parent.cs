using System;
using System.Collections.Generic;


namespace ZofimPortalApp.Models
{
    public partial class Parent
    {
        public Parent()
        {
            Cadets = new HashSet<Cadet>();
        }

        public string FName { get; set; }
        public string LName { get; set; }
        public string PersonalId { get; set; }
        public int ShevetId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }

        public virtual Shevet Shevet { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Cadet> Cadets { get; set; }
    }
}
