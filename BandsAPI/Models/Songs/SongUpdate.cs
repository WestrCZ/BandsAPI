using BandsAPI.Api.Utilities;
using BandsAPI.Data.Entities;
using Newtonsoft.Json;
namespace BandsAPI.Api.Models.Songs;
public class SongUpdate
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("authorId")]
    public Guid AuthorId { get; set; }
}
public static class SongUpdateExtensions
{
    public static SongUpdate ToUpdate(this AppMapper mapper, Song source)
    {
        return new SongUpdate
        {
            Id = source.Id,
            Name = source.Name,
            AuthorId = source.AuthorId,
        };
    }
    public static void ApplyUpdate(this AppMapper mapper, SongUpdate source, Song target)
    {
        target.Name = source.Name;
        target.AuthorId = source.AuthorId;
    }
}