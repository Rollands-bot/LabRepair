using LabRepair.Data;
using LabRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Controllers;

public class KomputerController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(int? labId, KondisiKomputer? kondisi)
    {
        var q = db.Komputer.Include(k => k.Lab).AsQueryable();
        if (labId.HasValue) q = q.Where(k => k.LabId == labId);
        if (kondisi.HasValue) q = q.Where(k => k.Kondisi == kondisi);

        ViewBag.LabList = new SelectList(await db.Lab.OrderBy(l => l.Kode).ToListAsync(), "Id", "Nama", labId);
        ViewBag.LabId = labId;
        ViewBag.Kondisi = kondisi;
        return View(await q.OrderBy(k => k.KodePc).ToListAsync());
    }

    // Detail komputer + riwayat semua laporan & perbaikannya
    public async Task<IActionResult> Riwayat(int id)
    {
        var pc = await db.Komputer.Include(k => k.Lab)
            .Include(k => k.Laporan).ThenInclude(l => l.Pelapor)
            .Include(k => k.Laporan).ThenInclude(l => l.Perbaikan).ThenInclude(p => p.Teknisi)
            .FirstOrDefaultAsync(k => k.Id == id);
        return pc == null ? NotFound() : View(pc);
    }

    [Authorize(Roles = nameof(Peran.Admin))]
    public async Task<IActionResult> Create(int? labId)
    {
        await IsiDropdownLab();
        return View("Form", new Komputer { LabId = labId ?? 0 });
    }

    [Authorize(Roles = nameof(Peran.Admin))]
    public async Task<IActionResult> Edit(int id)
    {
        var pc = await db.Komputer.FindAsync(id);
        if (pc == null) return NotFound();
        await IsiDropdownLab();
        return View("Form", pc);
    }

    [Authorize(Roles = nameof(Peran.Admin)), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Simpan(Komputer pc)
    {
        if (await db.Komputer.AnyAsync(k => k.KodePc == pc.KodePc && k.Id != pc.Id))
            ModelState.AddModelError(nameof(Komputer.KodePc), "Kode PC sudah dipakai.");
        if (!ModelState.IsValid)
        {
            await IsiDropdownLab();
            return View("Form", pc);
        }

        if (pc.Id == 0) db.Komputer.Add(pc); else db.Komputer.Update(pc);
        await db.SaveChangesAsync();
        TempData["Pesan"] = $"Komputer {pc.KodePc} berhasil disimpan.";
        return RedirectToAction(nameof(Index), new { labId = pc.LabId });
    }

    [Authorize(Roles = nameof(Peran.Admin)), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Hapus(int id)
    {
        var pc = await db.Komputer.Include(k => k.Laporan).FirstOrDefaultAsync(k => k.Id == id);
        if (pc == null) return NotFound();
        if (pc.Laporan.Count > 0)
            TempData["Error"] = $"{pc.KodePc} punya riwayat laporan, tidak bisa dihapus.";
        else
        {
            db.Komputer.Remove(pc);
            await db.SaveChangesAsync();
            TempData["Pesan"] = $"Komputer {pc.KodePc} dihapus.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task IsiDropdownLab() =>
        ViewBag.LabList = new SelectList(await db.Lab.OrderBy(l => l.Kode).ToListAsync(), "Id", "Nama");
}
