using LabRepair.Data;
using LabRepair.Models;
using LabRepair.Models.ViewModels;
using LabRepair.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Controllers;

public class LaporanController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(StatusLaporan? status, int? labId)
    {
        var q = db.LaporanKerusakan
            .Include(l => l.Komputer!).ThenInclude(k => k.Lab)
            .Include(l => l.Pelapor).Include(l => l.Teknisi)
            .AsQueryable();

        // Pelapor hanya melihat laporan miliknya sendiri
        if (User.PeranPengguna() == Peran.Pelapor)
            q = q.Where(l => l.PelaporId == User.IdPengguna());
        if (status.HasValue) q = q.Where(l => l.Status == status);
        if (labId.HasValue) q = q.Where(l => l.Komputer!.LabId == labId);

        ViewBag.LabList = new SelectList(await db.Lab.OrderBy(l => l.Kode).ToListAsync(), "Id", "Nama", labId);
        ViewBag.Status = status;
        return View(await q.OrderByDescending(l => l.TanggalLapor).ToListAsync());
    }

    public async Task<IActionResult> Create(int? komputerId)
    {
        await IsiDropdownKomputer();
        return View(new LaporanKerusakan { KomputerId = komputerId ?? 0 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("KomputerId,Deskripsi,Prioritas")] LaporanKerusakan laporan)
    {
        var pc = await db.Komputer.FindAsync(laporan.KomputerId);
        if (pc == null) ModelState.AddModelError(nameof(laporan.KomputerId), "Pilih komputer.");
        else if (await db.LaporanKerusakan.AnyAsync(l => l.KomputerId == pc.Id &&
                     (l.Status == StatusLaporan.Dilaporkan || l.Status == StatusLaporan.Diproses)))
            ModelState.AddModelError(nameof(laporan.KomputerId),
                $"{pc.KodePc} masih punya laporan yang belum selesai.");

        if (!ModelState.IsValid)
        {
            await IsiDropdownKomputer();
            return View(laporan);
        }

        laporan.PelaporId = User.IdPengguna();
        laporan.TanggalLapor = DateTime.Now;
        laporan.Status = StatusLaporan.Dilaporkan;
        pc!.Kondisi = StatusWorkflow.KondisiUntuk(StatusLaporan.Dilaporkan);
        db.LaporanKerusakan.Add(laporan);
        await db.SaveChangesAsync();

        TempData["Pesan"] = $"Laporan untuk {pc.KodePc} berhasil dikirim.";
        return RedirectToAction(nameof(Detail), new { id = laporan.Id });
    }

    public async Task<IActionResult> Detail(int id)
    {
        var laporan = await AmbilLaporan(id);
        if (laporan == null) return NotFound();
        if (User.PeranPengguna() == Peran.Pelapor && laporan.PelaporId != User.IdPengguna()) return Forbid();

        ViewBag.StatusPilihan = StatusWorkflow.StatusBerikutnya(laporan.Status, User.PeranPengguna());
        return View(laporan);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = $"{nameof(Peran.Admin)},{nameof(Peran.Teknisi)}")]
    public async Task<IActionResult> Proses(ProsesVm vm)
    {
        var laporan = await AmbilLaporan(vm.LaporanId);
        if (laporan == null) return NotFound();

        // Validasi di server: jangan percaya isi dropdown dari browser
        if (!StatusWorkflow.BolehUbah(laporan.Status, vm.StatusBaru, User.PeranPengguna()))
            ModelState.AddModelError(nameof(vm.StatusBaru),
                $"Tidak boleh mengubah status dari {laporan.Status.Tampil()} ke {vm.StatusBaru.Tampil()}.");

        if (!ModelState.IsValid)
        {
            TempData["Error"] = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Detail), new { id = vm.LaporanId });
        }

        var teknisiId = User.IdPengguna();
        laporan.Perbaikan.Add(new Perbaikan
        {
            TeknisiId = teknisiId,
            Tanggal = DateTime.Now,
            Tindakan = vm.Tindakan,
            Sparepart = vm.Sparepart,
            Biaya = vm.Biaya,
            StatusSetelah = vm.StatusBaru
        });
        laporan.TeknisiId ??= teknisiId;
        laporan.Status = vm.StatusBaru;
        laporan.TanggalSelesai = vm.StatusBaru is StatusLaporan.Selesai or StatusLaporan.TidakBisaDiperbaiki
            ? DateTime.Now : null;
        laporan.Komputer!.Kondisi = StatusWorkflow.KondisiUntuk(vm.StatusBaru);
        await db.SaveChangesAsync();

        TempData["Pesan"] = $"Perbaikan dicatat. Status sekarang: {vm.StatusBaru.Tampil()}.";
        return RedirectToAction(nameof(Detail), new { id = vm.LaporanId });
    }

    private Task<LaporanKerusakan?> AmbilLaporan(int id) => db.LaporanKerusakan
        .Include(l => l.Komputer!).ThenInclude(k => k.Lab)
        .Include(l => l.Pelapor).Include(l => l.Teknisi)
        .Include(l => l.Perbaikan).ThenInclude(p => p.Teknisi)
        .FirstOrDefaultAsync(l => l.Id == id);

    private async Task IsiDropdownKomputer()
    {
        var pcs = await db.Komputer.Include(k => k.Lab).OrderBy(k => k.KodePc).ToListAsync();
        ViewBag.KomputerList = pcs.Select(k => new SelectListItem
        {
            Value = k.Id.ToString(),
            Text = $"{k.KodePc} — {k.Lab!.Nama} ({k.Kondisi.Tampil()})",
            Group = null
        }).ToList();
    }
}
