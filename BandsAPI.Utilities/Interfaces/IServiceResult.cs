namespace BandsAPI.Utilities.Interfaces;

public interface IServiceResult
{
    public Dictionary<string, string> Errors { get; set; }
}
