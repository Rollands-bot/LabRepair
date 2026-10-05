using System.ComponentModel.DataAnnotations;

namespace LabRepair.Models;

public class Pengguna
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = "";

    [Required, StringLength(100), Display(Name = "Nama Lengkap")]
    public string NamaLengkap { get; set; } = "";

    [Required]
    public string PasswordHash { get; set; } = "";

    public Peran Peran { get; set; }
}
