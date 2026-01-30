using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PMCSystem_Backend.Entities.TempEntities;

public partial class PmcContext : DbContext
{
    public PmcContext()
    {
    }

    public PmcContext(DbContextOptions<PmcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<S3dCommonCodeListHierarchy> S3dCommonCodeListHierarchies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCommonCodeListHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_CodeListHierarchy_PK");

            entity.ToTable("S3D_Common_CodeListHierarchy");

            entity.HasIndex(e => e.CodeListTableId, "DSP_CodeListHierarchy_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CodeListTableId).HasColumnName("CodeListTableID");
            entity.Property(e => e.ParentCodeListTableId).HasColumnName("ParentCodeListTableID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
