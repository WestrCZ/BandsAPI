using System.ComponentModel.DataAnnotations.Schema;
namespace BandsAPI.Data.Entities;
[Table(nameof(Song))]
public class Song
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Author Author { get; set; } = null!;
    public Guid AuthorId { get; set; }
}
