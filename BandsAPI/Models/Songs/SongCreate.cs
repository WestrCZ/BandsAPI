using BandsAPI.Utilities.Interfaces;
using BandsAPI.Data.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BandsAPI.Api.Models.Songs;
public class SongCreate
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("authorId")]
    public Guid AuthorId { get; set; }
}
public static class SongCreateExtensions
{
    public static Song FromCreate(this IAppMapper mapper, SongCreate source)
    {
        return new Song
        {
            Id = Guid.NewGuid(),
            Name = source.Name,
            AuthorId = source.AuthorId,
        };
    }
}
