using LabRepair.Models;

namespace LabRepair.Services;

// Aturan alur status laporan kerusakan.
// Dipakai di dua tempat: untuk mengisi dropdown "Ubah Status Menjadi" di halaman detail,
// dan untuk validasi di server saat form proses dikirim (supaya tidak bisa diakali lewat request).
public static class StatusWorkflow
{
    /// <summary>
    /// Status apa saja yang boleh dipilih berikutnya, berdasarkan status sekarang dan peran pengguna.
    /// Kembalikan list kosong kalau pengguna tidak boleh mengubah status sama sekali.
    /// Boleh menyertakan status yang sama (mis. Diproses -> Diproses) untuk mencatat progres tanpa ganti status.
    /// </summary>
    public static IReadOnlyList<StatusLaporan> StatusBerikutnya(StatusLaporan sekarang, Peran peran)
    {
        // TODO(lo yang isi): tentukan alur statusnya di sini.
        throw new NotImplementedException("StatusWorkflow.StatusBerikutnya belum diisi");
    }

    public static bool BolehUbah(StatusLaporan dari, StatusLaporan ke, Peran peran) =>
        StatusBerikutnya(dari, peran).Contains(ke);

    // Kondisi fisik komputer mengikuti status laporan terakhirnya
    public static KondisiKomputer KondisiUntuk(StatusLaporan status) => status switch
    {
        StatusLaporan.Dilaporkan => KondisiKomputer.Rusak,
        StatusLaporan.Diproses => KondisiKomputer.DalamPerbaikan,
        StatusLaporan.Selesai => KondisiKomputer.Baik,
        _ => KondisiKomputer.Rusak
    };
}
