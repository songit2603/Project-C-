using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<CTHD> CTHDs { get; set; }
        public virtual DbSet<HOADON> HOADONs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<LOAISANPHAM> LOAISANPHAMs { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<V_HOADON> V_HOADONs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HOADON>()
                .Property(e => e.THANHTIEN)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HOADON>()
                .Property(e => e.TIENKHACHHANGTRA)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HOADON>()
                .Property(e => e.TIENTHUA)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HOADON>()
                .HasMany(e => e.CTHDs)
                .WithOptional(e => e.HOADON)
                .WillCascadeOnDelete();

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<LOAISANPHAM>()
                .HasMany(e => e.SANPHAMs)
                .WithOptional(e => e.LOAISANPHAM)
                .WillCascadeOnDelete();

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.NGAYSINH)
                .IsFixedLength();

            modelBuilder.Entity<NHANVIEN>()
                .HasOptional(e => e.TAIKHOAN)
                .WithRequired(e => e.NHANVIEN);
        }
    }
}
