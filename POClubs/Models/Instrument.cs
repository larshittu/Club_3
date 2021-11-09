using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Models
{
    public partial class Instrument
    {
        public Instrument()
        {
            ArtistInstrument = new HashSet<ArtistInstrument>();
        }

        public int InstrumentId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ArtistInstrument> ArtistInstrument { get; set; }
    }
}
