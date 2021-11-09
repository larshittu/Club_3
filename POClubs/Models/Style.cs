using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Models
{
    public partial class Style
    {
        public Style()
        {
            ArtistStyle = new HashSet<ArtistStyle>();
            ClubStyle = new HashSet<ClubStyle>();
        }

        public string StyleName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ArtistStyle> ArtistStyle { get; set; }
        public virtual ICollection<ClubStyle> ClubStyle { get; set; }
    }
}
