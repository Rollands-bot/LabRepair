using LabRepair.Data;
using LabRepair.Models;
using LabRepair.Models.ViewModels;
using LabRepair.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Controllers;

[Authorize(Roles = nameof(Peran.Admin))]
public class PenggunaController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() =>
        View(await db.Pengguna.OrderBy(p => p.Peran).ThenBy(p => p.Username).ToListAsync());

    public IActionResult Create() => View(new PenggunaVm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PenggunaVm vm)
    {
        if (await db.Pengguna.AnyAsync(p => p.Username == vm.Username))
            ModelState.AddModelError(nameof(vm.Username), "Username sudah dipakai.");
        if (!ModelState.IsValid) return View(vm);

        var p = new Pengguna { Username = vm.Username, NamaLengkap = vm.NamaLengkap, Peran = vm.Peran };
        p.PasswordHash = new PasswordHasher<Pengguna>().HashPassword(p, vm.Password);
        db.Pengguna.Add(p);
        await db.SaveChangesAsync();
        TempData["Pesan"] = $"Pengguna {p.Username} ditambahkan.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Hapus(int id)
    {
        if (id == User.IdPengguna())
        {
            TempData["Error"] = "Tidak bisa menghapus akun sendiri.";
            return RedirectToAction(nameof(Index));
        }
        var p = await db.Pengguna.FindAsync(id);
        if (p == null) return NotFound();
        try
        {
            db.Pengguna.Remove(p);
            await db.SaveChangesAsync();
            TempData["Pesan"] = $"Pengguna {p.Username} dihapus.";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = $"{p.Username} sudah punya riwayat laporan/perbaikan, tidak bisa dihapus.";
        }
        return RedirectToAction(nameof(Index));
    }
}
