using BandsAPI.Api.Utilities;
using BandsAPI.Data.Entities;
using Newtonsoft.Json;

namespace BandsAPI.Api.Models.Authors;
public class AuthorCreate
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("description")]
    public string? Description { get; set; }
}
public static class AuthorCreateExtensions
{
    public static Author FromCreate(this AppMapper mapper, AuthorCreate source)
    {
        return new Author
        {
            Id = Guid.NewGuid(),
            Name = source.Name,
            Description = source.Description,
        };
    }
}
