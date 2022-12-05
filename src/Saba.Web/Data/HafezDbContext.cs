using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Saba.Web.Data;

public partial class HafezDbContext : DbContext
{
    public HafezDbContext()
    {
    }

    public HafezDbContext(DbContextOptions<HafezDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fal> Fals { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlite("data source=hafezdata.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fal>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("fal");

            entity.Property(e => e.Beit).HasColumnName("beit");
            entity.Property(e => e.Ghazal)
                .HasColumnType("INT")
                .HasColumnName("ghazal");
            entity.Property(e => e.Id)
                .HasColumnType("INT")
                .HasColumnName("id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
