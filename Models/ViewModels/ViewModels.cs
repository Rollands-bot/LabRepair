using System.ComponentModel.DataAnnotations;

namespace LabRepair.Models.ViewModels;

public class LoginVm
{
    [Required]
    public string Username { get; set; } = "";

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
}

public class PenggunaVm
{
    [Required, StringLength(50)]
    public string Username { get; set; } = "";

    [Required, StringLength(100), Display(Name = "Nama Lengkap")]
    public string NamaLengkap { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public Peran Peran { get; set; } = Peran.Pelapor;
}

public class ProsesVm
{
    public int LaporanId { get; set; }

    [Required, StringLength(500)]
    public string Tindakan { get; set; } = "";

    [StringLength(200), Display(Name = "Sparepart Diganti")]
    public string? Sparepart { get; set; }

    [Range(0, 100_000_000)]
    public decimal Biaya { get; set; }

    [Display(Name = "Ubah Status Menjadi")]
    public StatusLaporan StatusBaru { get; set; }
}

public class DashboardVm
{
    public int TotalKomputer { get; set; }
    public int KomputerRusak { get; set; }
    public int LaporanTerbuka { get; set; }
    public int SelesaiBulanIni { get; set; }
    // Laporan yang belum selesai, prioritas tertinggi & paling lama menunggu di atas
    public List<LaporanKerusakan> Antrean { get; set; } = new();
    public List<(Komputer Pc, int Jumlah)> SeringRusak { get; set; } = new();
}

public class RekapBarisVm
{
    public string Lab { get; set; } = "";
    public int Total { get; set; }
    public int Dilaporkan { get; set; }
    public int Diproses { get; set; }
    public int Selesai { get; set; }
    public int TidakBisa { get; set; }
    public decimal Biaya { get; set; }
}

public class RekapVm
{
    public int Bulan { get; set; }
    public int Tahun { get; set; }
    public int? LabId { get; set; }
    public List<RekapBarisVm> PerLab { get; set; } = new();
    public List<LaporanKerusakan> Detail { get; set; } = new();
}
