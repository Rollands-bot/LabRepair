using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabRepair.Migrations
{
    /// <inheritdoc />
    public partial class Awal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lab",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Lokasi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pengguna",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NamaLengkap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Peran = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pengguna", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Komputer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodePc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LabId = table.Column<int>(type: "int", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ram = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Penyimpanan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Os = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kondisi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komputer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komputer_Lab_LabId",
                        column: x => x.LabId,
                        principalTable: "Lab",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LaporanKerusakan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KomputerId = table.Column<int>(type: "int", nullable: false),
                    PelaporId = table.Column<int>(type: "int", nullable: false),
                    TeknisiId = table.Column<int>(type: "int", nullable: true),
                    Deskripsi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Prioritas = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TanggalLapor = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TanggalSelesai = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaporanKerusakan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaporanKerusakan_Komputer_KomputerId",
                        column: x => x.KomputerId,
                        principalTable: "Komputer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaporanKerusakan_Pengguna_PelaporId",
                        column: x => x.PelaporId,
                        principalTable: "Pengguna",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaporanKerusakan_Pengguna_TeknisiId",
                        column: x => x.TeknisiId,
                        principalTable: "Pengguna",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Perbaikan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaporanId = table.Column<int>(type: "int", nullable: false),
                    TeknisiId = table.Column<int>(type: "int", nullable: false),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tindakan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Sparepart = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Biaya = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    StatusSetelah = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perbaikan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perbaikan_LaporanKerusakan_LaporanId",
                        column: x => x.LaporanId,
                        principalTable: "LaporanKerusakan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Perbaikan_Pengguna_TeknisiId",
                        column: x => x.TeknisiId,
                        principalTable: "Pengguna",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Komputer_KodePc",
                table: "Komputer",
                column: "KodePc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Komputer_LabId",
                table: "Komputer",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_Lab_Kode",
                table: "Lab",
                column: "Kode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaporanKerusakan_KomputerId",
                table: "LaporanKerusakan",
                column: "KomputerId");

            migrationBuilder.CreateIndex(
                name: "IX_LaporanKerusakan_PelaporId",
                table: "LaporanKerusakan",
                column: "PelaporId");

            migrationBuilder.CreateIndex(
                name: "IX_LaporanKerusakan_TeknisiId",
                table: "LaporanKerusakan",
                column: "TeknisiId");

            migrationBuilder.CreateIndex(
                name: "IX_Pengguna_Username",
                table: "Pengguna",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Perbaikan_LaporanId",
                table: "Perbaikan",
                column: "LaporanId");

            migrationBuilder.CreateIndex(
                name: "IX_Perbaikan_TeknisiId",
                table: "Perbaikan",
                column: "TeknisiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perbaikan");

            migrationBuilder.DropTable(
                name: "LaporanKerusakan");

            migrationBuilder.DropTable(
                name: "Komputer");

            migrationBuilder.DropTable(
                name: "Pengguna");

            migrationBuilder.DropTable(
                name: "Lab");
        }
    }
}
