# LabRepair — Sistem Informasi Perbaikan Komputer Lab

Tugas Tutorial **Program ASP** — ASP.NET Core MVC (.NET 10) + SQL Server + Entity Framework Core.

## Fitur
- Login dengan 3 peran: **Admin** (Kepala Lab), **Teknisi**, **Pelapor** (Asisten Lab)
- Data lab & data komputer per lab (CRUD, filter per lab/kondisi)
- Laporan kerusakan (pelapor hanya melihat laporannya sendiri)
- Proses perbaikan oleh teknisi: tindakan, sparepart, biaya, perubahan status
- Riwayat perbaikan per komputer
- Rekap laporan per bulan / per lab, bisa dicetak

## Struktur Database
`Lab` 1—N `Komputer` 1—N `LaporanKerusakan` 1—N `Perbaikan`; `Pengguna` sebagai pelapor/teknisi.

File database ada di folder `Database/` (pilih salah satu):
- **`LabRepairDb.sql`**: struktur tabel + data awal. Buka di SQL Server Management Studio, lalu **Execute (F5)**.
  Database `LabRepairDb` dibuat otomatis. Bisa dipakai di SQL Server/LocalDB versi apa pun.
- **`LabRepairDb.bak`**: backup database (SQL Server 2022 ke atas).
  Di SSMS: klik kanan **Databases → Restore Database… → Device**, pilih file ini, beri nama `LabRepairDb`.

Tanpa keduanya pun aplikasi tetap jalan: saat pertama dijalankan, database dibuat otomatis beserta data contoh.

## Cara Menjalankan (Windows + Visual Studio)
1. Buka `LabRepair.csproj` di Visual Studio 2022/2026.
2. Tekan **F5**. Database `LabRepairDb` otomatis dibuat di `(localdb)\MSSQLLocalDB` beserta data contoh.

## Cara Menjalankan (macOS, SQL Server di Docker)
```
docker start sql-lab
dotnet run --launch-profile http
```
Connection string Mac disimpan di user-secrets (`dotnet user-secrets list`), bukan di repo.

## Akun Demo
Username: `admin`, `teknisi`, `asisten` — password awal ada di `appsettings.json` → `Seed:PasswordAwal`.
