using BandsAPI.Utilities.Interfaces;
using BandsAPI.Data.Entities;
using Newtonsoft.Json;

namespace BandsAPI.Api.Models.Authors;
public class AuthorUpdate
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("description")]
    public string? Description { get; set; }
}
public static class AuthorUpdateExtensions
{
    public static AuthorUpdate ToUpdate(this IAppMapper mapper, Author source)
    {
        return new AuthorUpdate
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
        };
    }
    public static void ApplyUpdate(this IAppMapper mapper, AuthorUpdate source ,Author target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
    }
}
