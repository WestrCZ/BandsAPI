using BandsAPI.Api.Models.InnerModels;
using BandsAPI.Utilities.Interfaces;
using BandsAPI.Data.Entities;
using Newtonsoft.Json;
namespace BandsAPI.Api.Models.Songs;

public class SongDetail
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("author")]
    public  GenericInnerModel Author { get; set; } = null!;
}
public static class SongDetailExtensions
{
    public static SongDetail ToDetail(this IAppMapper mapper, Song source)
    {
        return new SongDetail
        {
            Id = source.Id,
            Name = source.Name,
            Author = new GenericInnerModel { Id = source.AuthorId, Name = source.Author!.Name },
        };
    }
    public static Song FromDetail(this IAppMapper mapper, SongDetail source)
    {
        return new()
        {
            Id = source.Id,
            Name = source.Name,
            Author = new Author { Id = source.Author.Id, Name = source.Author!.Name },
        };
    }
}
