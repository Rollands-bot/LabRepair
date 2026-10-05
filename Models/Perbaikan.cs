using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabRepair.Models;

// Satu baris = satu tindakan teknisi pada sebuah laporan (log riwayat)
public class Perbaikan
{
    public int Id { get; set; }

    public int LaporanId { get; set; }
    public LaporanKerusakan? Laporan { get; set; }

    public int TeknisiId { get; set; }
    public Pengguna? Teknisi { get; set; }

    public DateTime Tanggal { get; set; } = DateTime.Now;

    [Required, StringLength(500)]
    public string Tindakan { get; set; } = "";

    [StringLength(200), Display(Name = "Sparepart Diganti")]
    public string? Sparepart { get; set; }

    [Column(TypeName = "decimal(12,2)"), Range(0, 100_000_000)]
    public decimal Biaya { get; set; }

    [Display(Name = "Status Setelah")]
    public StatusLaporan StatusSetelah { get; set; }
}
