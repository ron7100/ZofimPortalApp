using System;
using System.Collections.Generic;

namespace ZofimPortalApp.Models
{
    public partial class Worker
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string PersonalId { get; set; }
        public int? ShevetId { get; set; }
        public string Role { get; set; }
        public int HanhagaId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }

        public virtual Hanhaga Hanhaga { get; set; }
        public virtual Shevet Shevet { get; set; }
        public virtual User User { get; set; }
    }
}
