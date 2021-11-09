using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Models
{
    public partial class ArtistInstrument
    {
        public int ArtistInstrumentId { get; set; }
        public int ArtistId { get; set; }
        public int InstrumentId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Instrument Instrument { get; set; }
    }
}
