using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models;

public partial class DfpowerbiDevContext : DbContext
{
    public DfpowerbiDevContext()
    {
    }

    public DfpowerbiDevContext(DbContextOptions<DfpowerbiDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hospital> Hospitals { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportHospitalMap> ReportHospitalMaps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserHospitalMap> UserHospitalMaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:dfpowebi-dev.database.windows.net,1433;Initial Catalog=dfpowerbi-dev;Persist Security Info=False;User ID=dfAdmin;Password=dfPowerbi@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hospital>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hospital__3214EC27BB2EA64D");

            entity.ToTable("Hospital");

            entity.HasIndex(e => e.Name, "UQ__Hospital__737584F6658E2813").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reports__3214EC27F80B40CB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ReportId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ReportID");
            entity.Property(e => e.WorkspaceId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("WorkspaceID");
        });

        modelBuilder.Entity<ReportHospitalMap>(entity =>
        {
            entity.HasKey(e => new { e.ReportId, e.HospitalId }).HasName("PK__ReportHo__363166BD106C66DD");

            entity.ToTable("ReportHospitalMap");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.HospitalId).HasColumnName("HospitalID");
            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Hospital).WithMany(p => p.ReportHospitalMaps)
                .HasForeignKey(d => d.HospitalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportHos__Hospi__08B54D69");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportHospitalMaps)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportHos__Repor__07C12930");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27F9FCF81E");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserHospitalMap>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.HospitalId }).HasName("PK__UserHosp__F404E2F4DB500EA3");

            entity.ToTable("UserHospitalMap");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.HospitalId).HasColumnName("HospitalID");
            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Hospital).WithMany(p => p.UserHospitalMaps)
                .HasForeignKey(d => d.HospitalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserHospi__Hospi__797309D9");

            entity.HasOne(d => d.User).WithMany(p => p.UserHospitalMaps)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserHospi__UserI__787EE5A0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
