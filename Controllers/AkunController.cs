using System.Security.Claims;
using LabRepair.Data;
using LabRepair.Models;
using LabRepair.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabRepair.Controllers;

public class AkunController(AppDbContext db) : Controller
{
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl) => View(new LoginVm { ReturnUrl = returnUrl });

    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await db.Pengguna.FirstOrDefaultAsync(p => p.Username == vm.Username);
        var valid = user != null && new PasswordHasher<Pengguna>()
            .VerifyHashedPassword(user, user.PasswordHash, vm.Password) != PasswordVerificationResult.Failed;
        if (!valid)
        {
            ModelState.AddModelError("", "Username atau password salah.");
            return View(vm);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user!.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Peran.ToString()),
            new("NamaLengkap", user.NamaLengkap)
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        return Url.IsLocalUrl(vm.ReturnUrl) ? Redirect(vm.ReturnUrl) : RedirectToAction("Index", "Home");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public IActionResult AksesDitolak() => View();
}
