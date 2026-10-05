using System.ComponentModel.DataAnnotations;

namespace LabRepair.Models;

public class LaporanKerusakan
{
    public int Id { get; set; }

    [Display(Name = "Komputer")]
    public int KomputerId { get; set; }
    public Komputer? Komputer { get; set; }

    public int PelaporId { get; set; }
    public Pengguna? Pelapor { get; set; }

    // Diisi saat teknisi pertama kali memproses laporan
    public int? TeknisiId { get; set; }
    public Pengguna? Teknisi { get; set; }

    [Required, StringLength(500), Display(Name = "Deskripsi Kerusakan")]
    public string Deskripsi { get; set; } = "";

    public Prioritas Prioritas { get; set; } = Prioritas.Sedang;

    public StatusLaporan Status { get; set; } = StatusLaporan.Dilaporkan;

    [Display(Name = "Tanggal Lapor")]
    public DateTime TanggalLapor { get; set; } = DateTime.Now;

    [Display(Name = "Tanggal Selesai")]
    public DateTime? TanggalSelesai { get; set; }

    public ICollection<Perbaikan> Perbaikan { get; set; } = new List<Perbaikan>();
}
