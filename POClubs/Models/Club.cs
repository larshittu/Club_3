using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Models
{
    public partial class Club
    {
        public Club()
        {
            ClubStyle = new HashSet<ClubStyle>();
            Contract = new HashSet<Contract>();
        }

        public int ClubId { get; set; }

        public virtual NameAddress ClubNavigation { get; set; }
        public virtual ICollection<ClubStyle> ClubStyle { get; set; }
        public virtual ICollection<Contract> Contract { get; set; }
    }
}
