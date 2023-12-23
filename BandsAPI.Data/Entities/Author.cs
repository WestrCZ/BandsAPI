using System.ComponentModel.DataAnnotations.Schema;

namespace BandsAPI.Data.Entities;
[Table(nameof(Author))]
public class Author
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Song>? Songs { get; set; } = new HashSet<Song>();
}
