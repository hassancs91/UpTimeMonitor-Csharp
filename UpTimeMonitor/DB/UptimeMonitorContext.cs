using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UpTimeMonitor
{
    public partial class UptimeMonitorContext : DbContext
    {
        public UptimeMonitorContext()
        {
        }

        public UptimeMonitorContext(DbContextOptions<UptimeMonitorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Uptime> Uptimes { get; set; } = null!;
        public virtual DbSet<Website> Websites { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=UptimeMonitor;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Uptime>(entity =>
            {
                entity.HasKey(e => e.Scanid)
                    .HasName("PK_records");

                entity.ToTable("uptime");

                entity.Property(e => e.Scanid).HasColumnName("scanid");

                entity.Property(e => e.ResponseTime).HasColumnName("responseTime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.Websiteid).HasColumnName("websiteid");
            });

            modelBuilder.Entity<Website>(entity =>
            {
                entity.ToTable("websites");

                entity.Property(e => e.Websiteid).HasColumnName("websiteid");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("url");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
