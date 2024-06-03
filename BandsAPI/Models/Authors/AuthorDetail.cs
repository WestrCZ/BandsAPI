using Newtonsoft.Json;
using BandsAPI.Utilities.Interfaces;
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
    public static AuthorDetail ToDetail(this IAppMapper mapper, Author source)
    {
        return new AuthorDetail
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
        };
    }
    public static Author FromDetail(this IAppMapper mapper, AuthorDetail source)
    {
        return new()
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
        };
    }
}
