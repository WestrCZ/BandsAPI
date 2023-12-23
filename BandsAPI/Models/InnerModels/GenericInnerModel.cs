using Newtonsoft.Json;

namespace BandsAPI.Api.Models.InnerModels;
public class GenericInnerModel
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
}
