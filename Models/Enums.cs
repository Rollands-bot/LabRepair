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

    public static string Badge(this StatusLaporan s) => s switch
    {
        StatusLaporan.Dilaporkan => "bg-warning text-dark",
        StatusLaporan.Diproses => "bg-info text-dark",
        StatusLaporan.Selesai => "bg-success",
        _ => "bg-secondary"
    };

    public static string Badge(this KondisiKomputer k) => k switch
    {
        KondisiKomputer.Baik => "bg-success",
        KondisiKomputer.Rusak => "bg-danger",
        _ => "bg-info text-dark"
    };

    public static string Badge(this Prioritas p) => p switch
    {
        Prioritas.Tinggi => "bg-danger",
        Prioritas.Sedang => "bg-warning text-dark",
        _ => "bg-light text-dark border"
    };
}
