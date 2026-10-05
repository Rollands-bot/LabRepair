using System.ComponentModel.DataAnnotations;

namespace LabRepair.Models;

public class Lab
{
    public int Id { get; set; }

    [Required, StringLength(10), Display(Name = "Kode Lab")]
    public string Kode { get; set; } = "";

    [Required, StringLength(100), Display(Name = "Nama Lab")]
    public string Nama { get; set; } = "";

    [StringLength(100)]
    public string? Lokasi { get; set; }

    public ICollection<Komputer> Komputer { get; set; } = new List<Komputer>();
}
