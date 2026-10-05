using LabRepair.Data;
using LabRepair.Models;
using LabRepair.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Controllers;

[Authorize(Roles = $"{nameof(Peran.Admin)},{nameof(Peran.Teknisi)}")]
public class RekapController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(int? bulan, int? tahun, int? labId)
    {
        var vm = new RekapVm
        {
            Bulan = bulan ?? DateTime.Now.Month,
            Tahun = tahun ?? DateTime.Now.Year,
            LabId = labId
        };
        var awal = new DateTime(vm.Tahun, vm.Bulan, 1);
        var akhir = awal.AddMonths(1);

        var q = db.LaporanKerusakan
            .Include(l => l.Komputer!).ThenInclude(k => k.Lab)
            .Include(l => l.Teknisi).Include(l => l.Perbaikan)
            .Where(l => l.TanggalLapor >= awal && l.TanggalLapor < akhir);
        if (labId.HasValue) q = q.Where(l => l.Komputer!.LabId == labId);

        vm.Detail = await q.OrderBy(l => l.TanggalLapor).ToListAsync();
        vm.PerLab = vm.Detail.GroupBy(l => l.Komputer!.Lab!.Nama).OrderBy(g => g.Key)
            .Select(g => new RekapBarisVm
            {
                Lab = g.Key,
                Total = g.Count(),
                Dilaporkan = g.Count(l => l.Status == StatusLaporan.Dilaporkan),
                Diproses = g.Count(l => l.Status == StatusLaporan.Diproses),
                Selesai = g.Count(l => l.Status == StatusLaporan.Selesai),
                TidakBisa = g.Count(l => l.Status == StatusLaporan.TidakBisaDiperbaiki),
                Biaya = g.SelectMany(l => l.Perbaikan).Sum(p => p.Biaya)
            }).ToList();

        ViewBag.LabList = new SelectList(await db.Lab.OrderBy(l => l.Kode).ToListAsync(), "Id", "Nama", labId);
        return View(vm);
    }
}
