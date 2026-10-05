using LabRepair.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db, IConfiguration config)
    {
        db.Database.Migrate();
        if (db.Pengguna.Any()) return;

        var hasher = new PasswordHasher<Pengguna>();
        var passwordAwal = config["Seed:PasswordAwal"] ?? "ganti-saya";
        Pengguna Buat(string username, string nama, Peran peran)
        {
            var p = new Pengguna { Username = username, NamaLengkap = nama, Peran = peran };
            p.PasswordHash = hasher.HashPassword(p, passwordAwal);
            return p;
        }

        var admin = Buat("admin", "Kepala Laboratorium", Peran.Admin);
        var teknisi = Buat("teknisi", "Budi Teknisi", Peran.Teknisi);
        var pelapor = Buat("asisten", "Asisten Lab", Peran.Pelapor);
        db.Pengguna.AddRange(admin, teknisi, pelapor);

        var labs = new[]
        {
            new Lab { Kode = "LAB-1", Nama = "Lab Komputer 1", Lokasi = "Gedung A Lt. 2" },
            new Lab { Kode = "LAB-2", Nama = "Lab Komputer 2", Lokasi = "Gedung A Lt. 3" },
            new Lab { Kode = "LAB-JAR", Nama = "Lab Jaringan", Lokasi = "Gedung B Lt. 1" },
        };
        db.Lab.AddRange(labs);

        foreach (var lab in labs)
            for (var i = 1; i <= 8; i++)
                lab.Komputer.Add(new Komputer
                {
                    KodePc = $"{lab.Kode}-PC{i:00}",
                    Processor = "Intel Core i5-12400",
                    Ram = "16 GB",
                    Penyimpanan = "SSD 512 GB",
                    Os = "Windows 11 Pro"
                });
        db.SaveChanges();

        // Contoh data laporan supaya dashboard & rekap tidak kosong saat demo
        var pc = labs[0].Komputer.ToList();
        var l1 = new LaporanKerusakan { Komputer = pc[2], Pelapor = pelapor, Deskripsi = "Monitor tidak menyala, lampu power berkedip oranye.", Prioritas = Prioritas.Tinggi, TanggalLapor = DateTime.Now.AddDays(-1) };
        var l2 = new LaporanKerusakan { Komputer = pc[5], Pelapor = pelapor, Teknisi = teknisi, Deskripsi = "Keyboard beberapa tombol tidak berfungsi.", Prioritas = Prioritas.Rendah, Status = StatusLaporan.Diproses, TanggalLapor = DateTime.Now.AddDays(-3) };
        var l3 = new LaporanKerusakan { Komputer = pc[2], Pelapor = pelapor, Teknisi = teknisi, Deskripsi = "PC sering restart sendiri.", Prioritas = Prioritas.Sedang, Status = StatusLaporan.Selesai, TanggalLapor = DateTime.Now.AddDays(-10), TanggalSelesai = DateTime.Now.AddDays(-8) };
        l2.Perbaikan.Add(new Perbaikan { Teknisi = teknisi, Tindakan = "Pengecekan keyboard, menunggu stok pengganti.", StatusSetelah = StatusLaporan.Diproses, Tanggal = DateTime.Now.AddDays(-2) });
        l3.Perbaikan.Add(new Perbaikan { Teknisi = teknisi, Tindakan = "Ganti PSU dan bersihkan debu.", Sparepart = "PSU 450W", Biaya = 350000, StatusSetelah = StatusLaporan.Selesai, Tanggal = DateTime.Now.AddDays(-8) });
        pc[2].Kondisi = KondisiKomputer.Rusak;
        pc[5].Kondisi = KondisiKomputer.DalamPerbaikan;
        db.LaporanKerusakan.AddRange(l1, l2, l3);
        db.SaveChanges();
    }
}
