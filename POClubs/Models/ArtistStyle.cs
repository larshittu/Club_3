using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Models
{
    public partial class ArtistStyle
    {
        public int ArtistId { get; set; }
        public string StyleName { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Style StyleNameNavigation { get; set; }
    }
}
