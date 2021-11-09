using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Models
{
    public partial class Contract
    {
        public int Contract1 { get; set; }
        public DateTime StartDate { get; set; }
        public int ArtistId { get; set; }
        public int ClubId { get; set; }
        public double PricePerPerformance { get; set; }
        public int NumberPerformances { get; set; }
        public double TotalPrice { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Club Club { get; set; }
    }
}
