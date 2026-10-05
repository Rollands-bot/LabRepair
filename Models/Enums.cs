using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LabRepair.Models;

public enum Peran
{
    Admin,
    Teknisi,
    Pelapor
}

public enum StatusLaporan
{
    Dilaporkan,
    Diproses,
    Selesai,
    [Display(Name = "Tidak Bisa Diperbaiki")]
    TidakBisaDiperbaiki
}

public enum Prioritas
{
    Rendah,
    Sedang,
    Tinggi
}

public enum KondisiKomputer
{
    Baik,
    Rusak,
    [Display(Name = "Dalam Perbaikan")]
    DalamPerbaikan
}

public static class EnumExtensions
{
    // Ambil teks dari [Display(Name)] kalau ada, kalau tidak pakai nama enum-nya
    public static string Tampil(this Enum value) =>
        value.GetType().GetMember(value.ToString()).First()
            .GetCustomAttribute<DisplayAttribute>()?.Name ?? value.ToString();

    // Class CSS di wwwroot/css/site.css (lihat DESIGN.md)
    public static string Badge(this StatusLaporan s) => s switch
    {
        StatusLaporan.Dilaporkan => "status status-dilaporkan",
        StatusLaporan.Diproses => "status status-diproses",
        StatusLaporan.Selesai => "status status-selesai",
        _ => "status status-tidak"
    };

    public static string Badge(this KondisiKomputer k) => k switch
    {
        KondisiKomputer.Baik => "status status-baik",
        KondisiKomputer.Rusak => "status status-rusak",
        _ => "status status-diproses"
    };

    public static string Badge(this Prioritas p) => p switch
    {
        Prioritas.Tinggi => "prioritas-tinggi",
        Prioritas.Sedang => "prioritas-sedang",
        _ => "prioritas-rendah"
    };
}
