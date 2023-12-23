using BandsAPI.Api.Models.InnerModels;
using BandsAPI.Api.Utilities;
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
    public static SongDetail ToDetail(this AppMapper mapper, Song source)
    {
        return new SongDetail
        {
            Id = source.Id,
            Name = source.Name,
            Author = new GenericInnerModel { Id = source.AuthorId, Name = source.Author!.Name},
        };
    }
}
