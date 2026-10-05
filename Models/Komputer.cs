using System.ComponentModel.DataAnnotations;

namespace LabRepair.Models;

public class Komputer
{
    public int Id { get; set; }

    [Required, StringLength(20), Display(Name = "Kode PC")]
    public string KodePc { get; set; } = "";

    [Display(Name = "Lab")]
    public int LabId { get; set; }
    public Lab? Lab { get; set; }

    [StringLength(100)]
    public string? Processor { get; set; }

    [StringLength(30), Display(Name = "RAM")]
    public string? Ram { get; set; }

    [StringLength(50)]
    public string? Penyimpanan { get; set; }

    [StringLength(50), Display(Name = "Sistem Operasi")]
    public string? Os { get; set; }

    public KondisiKomputer Kondisi { get; set; } = KondisiKomputer.Baik;

    public ICollection<LaporanKerusakan> Laporan { get; set; } = new List<LaporanKerusakan>();
}
