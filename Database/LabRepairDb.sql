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

