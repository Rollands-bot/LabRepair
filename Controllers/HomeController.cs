using System.Diagnostics;
using LabRepair.Data;
using LabRepair.Models;
using LabRepair.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var awalBulan = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var sering = await db.LaporanKerusakan
            .GroupBy(l => l.KomputerId)
            .Select(g => new { KomputerId = g.Key, Jumlah = g.Count() })
            .OrderByDescending(x => x.Jumlah).Take(5).ToListAsync();
        var pcs = await db.Komputer.Include(k => k.Lab)
            .Where(k => sering.Select(s => s.KomputerId).Contains(k.Id)).ToListAsync();

        var vm = new DashboardVm
        {
            TotalKomputer = await db.Komputer.CountAsync(),
            KomputerRusak = await db.Komputer.CountAsync(k => k.Kondisi != KondisiKomputer.Baik),
            LaporanTerbuka = await db.LaporanKerusakan.CountAsync(l =>
                l.Status == StatusLaporan.Dilaporkan || l.Status == StatusLaporan.Diproses),
            SelesaiBulanIni = await db.LaporanKerusakan.CountAsync(l =>
                l.Status == StatusLaporan.Selesai && l.TanggalSelesai >= awalBulan),
            LaporanTerbaru = await db.LaporanKerusakan
                .Include(l => l.Komputer!).ThenInclude(k => k.Lab)
                .OrderByDescending(l => l.TanggalLapor).Take(5).ToListAsync(),
            SeringRusak = sering.Select(s => (pcs.First(p => p.Id == s.KomputerId), s.Jumlah)).ToList()
        };
        return View(vm);
    }

    [AllowAnonymous, ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
