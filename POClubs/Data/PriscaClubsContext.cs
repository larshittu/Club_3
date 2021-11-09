using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using POClubs.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace POClubs.Data
{
    public partial class POClubsContext : DbContext
    {
        public POClubsContext()
        {
        }

        public POClubsContext(DbContextOptions<POClubsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<ArtistInstrument> ArtistInstrument { get; set; }
        public virtual DbSet<ArtistStyle> ArtistStyle { get; set; }
        public virtual DbSet<Club> Club { get; set; }
        public virtual DbSet<ClubStyle> ClubStyle { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<GroupMember> GroupMember { get; set; }
        public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<NameAddress> NameAddress { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Style> Style { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Clubs;User Id=sa;Password=Sterling@Dev;MultipleActiveResultSets=true;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("artist");

                entity.Property(e => e.ArtistId)
                    .HasColumnName("artistId")
                    .ValueGeneratedNever();

                entity.Property(e => e.MinimumHourlyRate).HasColumnName("minimumHourlyRate");

                entity.Property(e => e.NameAddressid).HasColumnName("nameAddressid");

                entity.HasOne(d => d.NameAddress)
                    .WithMany(p => p.Artist)
                    .HasForeignKey(d => d.NameAddressid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_artist_nameAddress");
            });

            modelBuilder.Entity<ArtistInstrument>(entity =>
            {
                entity.ToTable("artistInstrument");

                entity.Property(e => e.ArtistInstrumentId).HasColumnName("artistInstrumentId");

                entity.Property(e => e.ArtistId).HasColumnName("artistId");

                entity.Property(e => e.InstrumentId).HasColumnName("instrumentId");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistInstrument)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_artistInstrument_artist");

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.ArtistInstrument)
                    .HasForeignKey(d => d.InstrumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_artistInstrument_instrument");
            });

            modelBuilder.Entity<ArtistStyle>(entity =>
            {
                entity.HasKey(e => new { e.ArtistId, e.StyleName });

                entity.ToTable("artistStyle");

                entity.Property(e => e.ArtistId).HasColumnName("artistId");

                entity.Property(e => e.StyleName)
                    .HasColumnName("styleName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistStyle)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_artistStyle_artist");

                entity.HasOne(d => d.StyleNameNavigation)
                    .WithMany(p => p.ArtistStyle)
                    .HasForeignKey(d => d.StyleName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_artistStyle_style");
            });

            modelBuilder.Entity<Club>(entity =>
            {
                entity.ToTable("club");

                entity.Property(e => e.ClubId)
                    .HasColumnName("clubId")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.ClubNavigation)
                    .WithOne(p => p.Club)
                    .HasForeignKey<Club>(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_club_nameAddress");
            });

            modelBuilder.Entity<ClubStyle>(entity =>
            {
                entity.HasKey(e => new { e.ClubId, e.StyleName });

                entity.ToTable("clubStyle");

                entity.Property(e => e.ClubId).HasColumnName("clubId");

                entity.Property(e => e.StyleName)
                    .HasColumnName("styleName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.ClubStyle)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clubStyle_club");

                entity.HasOne(d => d.StyleNameNavigation)
                    .WithMany(p => p.ClubStyle)
                    .HasForeignKey(d => d.StyleName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_clubStyle_style");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.Contract1);

                entity.ToTable("contract");

                entity.Property(e => e.Contract1).HasColumnName("contract");

                entity.Property(e => e.ArtistId).HasColumnName("artistId");

                entity.Property(e => e.ClubId).HasColumnName("clubId");

                entity.Property(e => e.NumberPerformances).HasColumnName("numberPerformances");

                entity.Property(e => e.PricePerPerformance).HasColumnName("pricePerPerformance");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_contract_artist");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.ClubId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_contract_club");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryCode);

                entity.ToTable("country");

                entity.Property(e => e.CountryCode)
                    .HasColumnName("countryCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FederalSalesTax).HasColumnName("federalSalesTax");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhonePattern)
                    .HasColumnName("phonePattern")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostalPattern)
                    .HasColumnName("postalPattern")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProvinceTerminology)
                    .HasColumnName("provinceTerminology")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.ArtistIdGroup, e.ArtistIdMember })
                    .HasName("PK_groupMember_1");

                entity.ToTable("groupMember");

                entity.Property(e => e.ArtistIdGroup).HasColumnName("artistIdGroup");

                entity.Property(e => e.ArtistIdMember).HasColumnName("artistIdMember");

                entity.Property(e => e.DateJoined)
                    .HasColumnName("dateJoined")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateLeft)
                    .HasColumnName("dateLeft")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ArtistIdGroupNavigation)
                    .WithMany(p => p.GroupMemberArtistIdGroupNavigation)
                    .HasForeignKey(d => d.ArtistIdGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_groupMember_artist");

                entity.HasOne(d => d.ArtistIdMemberNavigation)
                    .WithMany(p => p.GroupMemberArtistIdMemberNavigation)
                    .HasForeignKey(d => d.ArtistIdMember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_groupMember_artist1");
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.ToTable("instrument");

                entity.Property(e => e.InstrumentId).HasColumnName("instrumentId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NameAddress>(entity =>
            {
                entity.ToTable("nameAddress");

                entity.Property(e => e.NameAddressId).HasColumnName("nameAddressId");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasColumnName("companyName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postalCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("provinceCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StreetAddress)
                    .HasColumnName("streetAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.NameAddress)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK_nameAddress_province");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.ProvinceCode);

                entity.ToTable("province");

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("provinceCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("countryCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FirstPostalLetter)
                    .HasColumnName("firstPostalLetter")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IncludesFederalTax).HasColumnName("includesFederalTax");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SalesTax).HasColumnName("salesTax");

                entity.Property(e => e.SalesTaxCode)
                    .HasColumnName("salesTaxCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Province)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_province_country");
            });

            modelBuilder.Entity<Style>(entity =>
            {
                entity.HasKey(e => e.StyleName);

                entity.ToTable("style");

                entity.Property(e => e.StyleName)
                    .HasColumnName("styleName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
