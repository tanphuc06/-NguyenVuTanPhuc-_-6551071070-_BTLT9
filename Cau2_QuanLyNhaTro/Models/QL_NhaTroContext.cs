using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Cau2_QuanLyNhaTro.Configuration;

namespace Cau2_QuanLyNhaTro.Models;

public partial class QL_NhaTroContext : DbContext
{
    public QL_NhaTroContext()
    {
    }

    public QL_NhaTroContext(DbContextOptions<QL_NhaTroContext> options)
        : base(options)
    {
    }

    public virtual DbSet<KhachThue> KhachThues { get; set; }
    public virtual DbSet<PhongTro> PhongTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connStr = AppSettings.Load().ConnectionStrings.DefaultConnection;
            optionsBuilder.UseSqlServer(connStr);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KhachThue>(entity =>
        {
            entity.HasKey(e => e.MaKhach);
            entity.ToTable("KhachThue");

            entity.Property(e => e.GioiTinh).HasMaxLength(5);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15).IsUnicode(false);
            entity.Property(e => e.TenPhuongXa).HasMaxLength(100);
            entity.Property(e => e.TenTinh).HasMaxLength(100);

            entity.HasOne(d => d.MaPhongNavigation)
                .WithMany(p => p.KhachThues)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK_KhachThue_PhongTro");
        });

        modelBuilder.Entity<PhongTro>(entity =>
        {
            entity.HasKey(e => e.MaPhong);
            entity.ToTable("PhongTro");

            entity.Property(e => e.GiaThue).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TenPhong).HasMaxLength(20);
            entity.Property(e => e.TinhTrang).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
