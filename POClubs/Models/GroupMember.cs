using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Models
{
    public partial class GroupMember
    {
        public int ArtistIdGroup { get; set; }
        public int ArtistIdMember { get; set; }
        public DateTime? DateJoined { get; set; }
        public DateTime? DateLeft { get; set; }

        public virtual Artist ArtistIdGroupNavigation { get; set; }
        public virtual Artist ArtistIdMemberNavigation { get; set; }
    }
}
