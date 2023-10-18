using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PaiPaiGO.Models;

public partial class PaiPaiGoContext : DbContext
{
    public PaiPaiGoContext()
    {
    }

    public PaiPaiGoContext(DbContextOptions<PaiPaiGoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abandoned> Abandoneds { get; set; }

    public virtual DbSet<Caption> Captions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<County> Counties { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<Opinion> Opinions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=PaiPaiGO;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abandoned>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__Abandone__66DFB854023F93B8");

            entity.ToTable("Abandoned");

            entity.Property(e => e.MissionId)
                .ValueGeneratedNever()
                .HasColumnName("MissionID");
            entity.Property(e => e.Date)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MemberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MemberID");

            entity.HasOne(d => d.Member).WithMany(p => p.Abandoneds)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Abandoned__Membe__2C3393D0");

            entity.HasOne(d => d.Mission).WithOne(p => p.Abandoned)
                .HasForeignKey<Abandoned>(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Abandoned__Missi__2B3F6F97");
        });

        modelBuilder.Entity<Caption>(entity =>
        {
            entity.HasKey(e => e.CaptionId).HasName("PK__Caption__428BB426E7E6813F");

            entity.ToTable("Caption");

            entity.Property(e => e.CaptionId)
                .ValueGeneratedNever()
                .HasColumnName("CaptionID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.TagName).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Captions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Caption__Categor__5165187F");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BBE59CE6C");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<County>(entity =>
        {
            entity.HasKey(e => e.Postcode).HasName("PK__County__476398EA9448B642");

            entity.ToTable("County");

            entity.Property(e => e.Postcode).HasMaxLength(10);
            entity.Property(e => e.Area).HasMaxLength(10);
            entity.Property(e => e.City).HasMaxLength(10);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Member__0CF04B389DC0A59D");

            entity.ToTable("Member");

            entity.Property(e => e.MemberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MemberID");
            entity.Property(e => e.Salt).HasMaxLength(100);
            entity.Property(e => e.Gearing).HasMaxLength(10);
            entity.Property(e => e.MemberAddress).HasMaxLength(100);
            entity.Property(e => e.MemberCity).HasMaxLength(10);
            entity.Property(e => e.MemberEmail).HasMaxLength(50);
            entity.Property(e => e.MemberName).HasMaxLength(100);
            entity.Property(e => e.MemberPassword).HasMaxLength(50);
            entity.Property(e => e.MemberPhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MemberPostcode).HasMaxLength(10);
            entity.Property(e => e.MemberStatus).HasMaxLength(50);
            entity.Property(e => e.MemberTownship).HasMaxLength(10);

            entity.HasOne(d => d.MemberPostcodeNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.MemberPostcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__MemberPo__286302EC");
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__Mission__66DFB854301AF33F");

            entity.ToTable("Mission");

            entity.Property(e => e.MissionId)
                .ValueGeneratedNever()
                .HasColumnName("MissionID");
            entity.Property(e => e.AcceptMemberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("AcceptMemberID");
            entity.Property(e => e.AcceptTime).HasMaxLength(255);
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.DeadlineDate).HasColumnType("date");
            entity.Property(e => e.DeliveryDate).HasColumnType("date");
            entity.Property(e => e.DeliveryMethod).HasMaxLength(50);
            entity.Property(e => e.ExecutionLocation).HasMaxLength(50);
            entity.Property(e => e.FormattedMissionAmount).HasMaxLength(50);
            entity.Property(e => e.LocationCity).HasMaxLength(50);
            entity.Property(e => e.LocationDistrict).HasMaxLength(50);
            entity.Property(e => e.MissionAmount).HasColumnType("money");
            entity.Property(e => e.MissionName).HasMaxLength(255);
            entity.Property(e => e.MissionStatus).HasMaxLength(50);
            entity.Property(e => e.OrderMemberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("OrderMemberID");
            entity.Property(e => e.OrderTime).HasColumnType("datetime");
            entity.Property(e => e.Postcode).HasMaxLength(10);
            entity.Property(e => e.Tags).HasMaxLength(50);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Missions)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mission_Category");
        });

        modelBuilder.Entity<Opinion>(entity =>
        {
            entity.HasKey(e => e.Ratingnumber).HasName("PK__Opinion__44BF21CD122ACDB2");

            entity.ToTable("Opinion");

            entity.Property(e => e.Ratingnumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Content)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MissionId).HasColumnName("MissionID");
            entity.Property(e => e.ReportMemberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ReportMemberID");
            entity.Property(e => e.ReportedMemberId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ReportedMemberID");
            entity.Property(e => e.State)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Mission).WithMany(p => p.Opinions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Opinion__Mission__6FE99F9F");

            entity.HasOne(d => d.ReportMember).WithMany(p => p.OpinionReportMembers)
                .HasForeignKey(d => d.ReportMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Opinion__ReportM__70DDC3D8");

            entity.HasOne(d => d.ReportedMember).WithMany(p => p.OpinionReportedMembers)
                .HasForeignKey(d => d.ReportedMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Opinion__Reporte__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
