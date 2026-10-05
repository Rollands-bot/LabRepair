using LabRepair.Data;
using LabRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Controllers;

[Authorize(Roles = nameof(Peran.Admin))]
public class LabController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await db.Lab.Include(l => l.Komputer).OrderBy(l => l.Kode).ToListAsync());

    public IActionResult Create() => View("Form", new Lab());

    public async Task<IActionResult> Edit(int id)
    {
        var lab = await db.Lab.FindAsync(id);
        return lab == null ? NotFound() : View("Form", lab);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Simpan(Lab lab)
    {
        if (await db.Lab.AnyAsync(l => l.Kode == lab.Kode && l.Id != lab.Id))
            ModelState.AddModelError(nameof(Lab.Kode), "Kode lab sudah dipakai.");
        if (!ModelState.IsValid) return View("Form", lab);

        if (lab.Id == 0) db.Lab.Add(lab); else db.Lab.Update(lab);
        await db.SaveChangesAsync();
        TempData["Pesan"] = $"Lab {lab.Nama} berhasil disimpan.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Hapus(int id)
    {
        var lab = await db.Lab.Include(l => l.Komputer).FirstOrDefaultAsync(l => l.Id == id);
        if (lab == null) return NotFound();
        if (lab.Komputer.Count > 0)
            TempData["Error"] = $"Lab {lab.Nama} masih punya {lab.Komputer.Count} komputer, pindahkan/hapus dulu.";
        else
        {
            db.Lab.Remove(lab);
            await db.SaveChangesAsync();
            TempData["Pesan"] = $"Lab {lab.Nama} dihapus.";
        }
        return RedirectToAction(nameof(Index));
    }
}
