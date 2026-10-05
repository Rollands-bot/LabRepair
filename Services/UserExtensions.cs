using System.Security.Claims;
using LabRepair.Models;

namespace LabRepair.Services;

public static class UserExtensions
{
    public static int IdPengguna(this ClaimsPrincipal user) =>
        int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static Peran PeranPengguna(this ClaimsPrincipal user) =>
        Enum.Parse<Peran>(user.FindFirstValue(ClaimTypes.Role)!);

    public static string NamaPengguna(this ClaimsPrincipal user) =>
        user.FindFirstValue("NamaLengkap") ?? user.Identity?.Name ?? "";
}
