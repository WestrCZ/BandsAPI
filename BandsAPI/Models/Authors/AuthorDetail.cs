using Newtonsoft.Json;
using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Utilities;
using BandsAPI.Data.Entities;

namespace BandsAPI.Api.Models.Authors;

public class AuthorDetail
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("description")]
    public string? Description { get; set; }
}
public static class AuthorDetailExtensions
{
    public static AuthorDetail ToDetail(this AppMapper mapper, Author source)
    {
        return new AuthorDetail
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
        };
    }
}
