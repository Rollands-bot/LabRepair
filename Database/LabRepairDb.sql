-- =============================================================
-- Database LabRepair: Sistem Informasi Perbaikan Komputer Lab
-- Berisi struktur tabel + data awal (3 lab, 24 komputer, 3 akun, 3 laporan contoh)
-- Cara pakai: buka di SQL Server Management Studio lalu Execute (F5)
-- =============================================================
IF DB_ID(N'LabRepairDb') IS NULL CREATE DATABASE [LabRepairDb];
GO
USE [LabRepairDb];
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Lab] (
    [Id] int NOT NULL IDENTITY,
    [Kode] nvarchar(10) NOT NULL,
    [Nama] nvarchar(100) NOT NULL,
    [Lokasi] nvarchar(100) NULL,
    CONSTRAINT [PK_Lab] PRIMARY KEY ([Id])
);

CREATE TABLE [Pengguna] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [NamaLengkap] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Peran] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Pengguna] PRIMARY KEY ([Id])
);

CREATE TABLE [Komputer] (
    [Id] int NOT NULL IDENTITY,
    [KodePc] nvarchar(20) NOT NULL,
    [LabId] int NOT NULL,
    [Processor] nvarchar(100) NULL,
    [Ram] nvarchar(30) NULL,
    [Penyimpanan] nvarchar(50) NULL,
    [Os] nvarchar(50) NULL,
    [Kondisi] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Komputer] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Komputer_Lab_LabId] FOREIGN KEY ([LabId]) REFERENCES [Lab] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [LaporanKerusakan] (
    [Id] int NOT NULL IDENTITY,
    [KomputerId] int NOT NULL,
    [PelaporId] int NOT NULL,
    [TeknisiId] int NULL,
    [Deskripsi] nvarchar(500) NOT NULL,
    [Prioritas] nvarchar(10) NOT NULL,
    [Status] nvarchar(30) NOT NULL,
    [TanggalLapor] datetime2 NOT NULL,
    [TanggalSelesai] datetime2 NULL,
    CONSTRAINT [PK_LaporanKerusakan] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LaporanKerusakan_Komputer_KomputerId] FOREIGN KEY ([KomputerId]) REFERENCES [Komputer] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_LaporanKerusakan_Pengguna_PelaporId] FOREIGN KEY ([PelaporId]) REFERENCES [Pengguna] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_LaporanKerusakan_Pengguna_TeknisiId] FOREIGN KEY ([TeknisiId]) REFERENCES [Pengguna] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Perbaikan] (
    [Id] int NOT NULL IDENTITY,
    [LaporanId] int NOT NULL,
    [TeknisiId] int NOT NULL,
    [Tanggal] datetime2 NOT NULL,
    [Tindakan] nvarchar(500) NOT NULL,
    [Sparepart] nvarchar(200) NULL,
    [Biaya] decimal(12,2) NOT NULL,
    [StatusSetelah] nvarchar(30) NOT NULL,
    CONSTRAINT [PK_Perbaikan] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Perbaikan_LaporanKerusakan_LaporanId] FOREIGN KEY ([LaporanId]) REFERENCES [LaporanKerusakan] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Perbaikan_Pengguna_TeknisiId] FOREIGN KEY ([TeknisiId]) REFERENCES [Pengguna] ([Id]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_Komputer_KodePc] ON [Komputer] ([KodePc]);

CREATE INDEX [IX_Komputer_LabId] ON [Komputer] ([LabId]);

CREATE UNIQUE INDEX [IX_Lab_Kode] ON [Lab] ([Kode]);

CREATE INDEX [IX_LaporanKerusakan_KomputerId] ON [LaporanKerusakan] ([KomputerId]);

CREATE INDEX [IX_LaporanKerusakan_PelaporId] ON [LaporanKerusakan] ([PelaporId]);

CREATE INDEX [IX_LaporanKerusakan_TeknisiId] ON [LaporanKerusakan] ([TeknisiId]);

CREATE UNIQUE INDEX [IX_Pengguna_Username] ON [Pengguna] ([Username]);

CREATE INDEX [IX_Perbaikan_LaporanId] ON [Perbaikan] ([LaporanId]);

CREATE INDEX [IX_Perbaikan_TeknisiId] ON [Perbaikan] ([TeknisiId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261005100029_Awal', N'10.0.12');

COMMIT;
GO

-- ===================== DATA AWAL =====================
-- Data Lab (3 baris)
SET IDENTITY_INSERT [Lab] ON;
INSERT INTO [Lab] ([Id], [Kode], [Nama], [Lokasi]) VALUES (1, N'LAB-1', N'Lab Komputer 1', N'Gedung A Lt. 2');
INSERT INTO [Lab] ([Id], [Kode], [Nama], [Lokasi]) VALUES (2, N'LAB-2', N'Lab Komputer 2', N'Gedung A Lt. 3');
INSERT INTO [Lab] ([Id], [Kode], [Nama], [Lokasi]) VALUES (3, N'LAB-JAR', N'Lab Jaringan', N'Gedung B Lt. 1');
SET IDENTITY_INSERT [Lab] OFF;
GO

-- Data Pengguna (3 baris)
SET IDENTITY_INSERT [Pengguna] ON;
INSERT INTO [Pengguna] ([Id], [Username], [NamaLengkap], [PasswordHash], [Peran]) VALUES (1, N'admin', N'Kepala Laboratorium', N'AQAAAAIAAYagAAAAEDilgmbHUSRZtYKqiKvehB7QA33HIOEdlrniyfkjLvlkOAgo/GzFRSTZg4RBgsqmZg==', N'Admin');
INSERT INTO [Pengguna] ([Id], [Username], [NamaLengkap], [PasswordHash], [Peran]) VALUES (2, N'teknisi', N'Budi Teknisi', N'AQAAAAIAAYagAAAAEHvl6Y+qJtBS4SIWu52P0Fe271YDsbOpZT386vIP01ZlOzDEZZuUnAOILJgd4nR12A==', N'Teknisi');
INSERT INTO [Pengguna] ([Id], [Username], [NamaLengkap], [PasswordHash], [Peran]) VALUES (3, N'asisten', N'Asisten Lab', N'AQAAAAIAAYagAAAAEFyJBElA8kQzxNQpDHLgd1K7VHvPiqOCGOccZy0a3x5bvjgPlCxgzTRvz8tELvM8Gw==', N'Pelapor');
SET IDENTITY_INSERT [Pengguna] OFF;
GO

-- Data Komputer (24 baris)
SET IDENTITY_INSERT [Komputer] ON;
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (1, N'LAB-1-PC01', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (2, N'LAB-1-PC02', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (3, N'LAB-1-PC03', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Rusak');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (4, N'LAB-1-PC04', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (5, N'LAB-1-PC05', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (6, N'LAB-1-PC06', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'DalamPerbaikan');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (7, N'LAB-1-PC07', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (8, N'LAB-1-PC08', 1, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (9, N'LAB-2-PC01', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (10, N'LAB-2-PC02', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (11, N'LAB-2-PC03', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (12, N'LAB-2-PC04', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (13, N'LAB-2-PC05', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (14, N'LAB-2-PC06', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (15, N'LAB-2-PC07', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (16, N'LAB-2-PC08', 2, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (17, N'LAB-JAR-PC01', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (18, N'LAB-JAR-PC02', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (19, N'LAB-JAR-PC03', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (20, N'LAB-JAR-PC04', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (21, N'LAB-JAR-PC05', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (22, N'LAB-JAR-PC06', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (23, N'LAB-JAR-PC07', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
INSERT INTO [Komputer] ([Id], [KodePc], [LabId], [Processor], [Ram], [Penyimpanan], [Os], [Kondisi]) VALUES (24, N'LAB-JAR-PC08', 3, N'Intel Core i5-12400', N'16 GB', N'SSD 512 GB', N'Windows 11 Pro', N'Baik');
SET IDENTITY_INSERT [Komputer] OFF;
GO

-- Data LaporanKerusakan (3 baris)
SET IDENTITY_INSERT [LaporanKerusakan] ON;
INSERT INTO [LaporanKerusakan] ([Id], [KomputerId], [PelaporId], [TeknisiId], [Deskripsi], [Prioritas], [Status], [TanggalLapor], [TanggalSelesai]) VALUES (1, 3, 3, NULL, N'Monitor tidak menyala, lampu power berkedip oranye.', N'Tinggi', N'Dilaporkan', N'2026-10-04T17:38:02.9825450', NULL);
INSERT INTO [LaporanKerusakan] ([Id], [KomputerId], [PelaporId], [TeknisiId], [Deskripsi], [Prioritas], [Status], [TanggalLapor], [TanggalSelesai]) VALUES (2, 6, 3, 2, N'Keyboard beberapa tombol tidak berfungsi.', N'Rendah', N'Diproses', N'2026-10-02T17:38:02.9825780', NULL);
INSERT INTO [LaporanKerusakan] ([Id], [KomputerId], [PelaporId], [TeknisiId], [Deskripsi], [Prioritas], [Status], [TanggalLapor], [TanggalSelesai]) VALUES (3, 3, 3, 2, N'PC sering restart sendiri.', N'Sedang', N'Selesai', N'2026-09-25T17:38:02.9825790', N'2026-09-27T17:38:02.9825790');
SET IDENTITY_INSERT [LaporanKerusakan] OFF;
GO

-- Data Perbaikan (2 baris)
SET IDENTITY_INSERT [Perbaikan] ON;
INSERT INTO [Perbaikan] ([Id], [LaporanId], [TeknisiId], [Tanggal], [Tindakan], [Sparepart], [Biaya], [StatusSetelah]) VALUES (1, 2, 2, N'2026-10-03T17:38:02.9826500', N'Pengecekan keyboard, menunggu stok pengganti.', NULL, 0.0, N'Diproses');
INSERT INTO [Perbaikan] ([Id], [LaporanId], [TeknisiId], [Tanggal], [Tindakan], [Sparepart], [Biaya], [StatusSetelah]) VALUES (2, 3, 2, N'2026-09-27T17:38:02.9826910', N'Ganti PSU dan bersihkan debu.', N'PSU 450W', 350000.0, N'Selesai');
SET IDENTITY_INSERT [Perbaikan] OFF;
GO
