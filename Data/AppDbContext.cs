using LabRepair.Models;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Lab> Lab => Set<Lab>();
    public DbSet<Komputer> Komputer => Set<Komputer>();
    public DbSet<Pengguna> Pengguna => Set<Pengguna>();
    public DbSet<LaporanKerusakan> LaporanKerusakan => Set<LaporanKerusakan>();
    public DbSet<Perbaikan> Perbaikan => Set<Perbaikan>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Lab>().HasIndex(l => l.Kode).IsUnique();
        mb.Entity<Komputer>().HasIndex(k => k.KodePc).IsUnique();
        mb.Entity<Pengguna>().HasIndex(p => p.Username).IsUnique();

        // Simpan enum sebagai teks supaya isi tabel mudah dibaca di SSMS
        mb.Entity<Komputer>().Property(k => k.Kondisi).HasConversion<string>().HasMaxLength(20);
        mb.Entity<Pengguna>().Property(p => p.Peran).HasConversion<string>().HasMaxLength(20);
        mb.Entity<LaporanKerusakan>().Property(l => l.Status).HasConversion<string>().HasMaxLength(30);
        mb.Entity<LaporanKerusakan>().Property(l => l.Prioritas).HasConversion<string>().HasMaxLength(10);
        mb.Entity<Perbaikan>().Property(p => p.StatusSetelah).HasConversion<string>().HasMaxLength(30);

        // Lab tidak bisa dihapus selama masih punya komputer
        mb.Entity<Komputer>().HasOne(k => k.Lab).WithMany(l => l.Komputer)
            .HasForeignKey(k => k.LabId).OnDelete(DeleteBehavior.Restrict);

        mb.Entity<LaporanKerusakan>().HasOne(l => l.Komputer).WithMany(k => k.Laporan)
            .HasForeignKey(l => l.KomputerId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<LaporanKerusakan>().HasOne(l => l.Pelapor).WithMany()
            .HasForeignKey(l => l.PelaporId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<LaporanKerusakan>().HasOne(l => l.Teknisi).WithMany()
            .HasForeignKey(l => l.TeknisiId).OnDelete(DeleteBehavior.Restrict);

        mb.Entity<Perbaikan>().HasOne(p => p.Laporan).WithMany(l => l.Perbaikan)
            .HasForeignKey(p => p.LaporanId).OnDelete(DeleteBehavior.Cascade);
        mb.Entity<Perbaikan>().HasOne(p => p.Teknisi).WithMany()
            .HasForeignKey(p => p.TeknisiId).OnDelete(DeleteBehavior.Restrict);
    }
}
