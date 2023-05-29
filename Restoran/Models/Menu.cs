using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.Models;

public class Menu
{
    public int Id { get; set; }
    public string? Image { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Tag { get; set; } = null!;
}
