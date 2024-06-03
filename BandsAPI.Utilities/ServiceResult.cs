using BandsAPI.Utilities.Interfaces;

namespace BandsAPI.Utilities;

public class ServiceResult<T> : IServiceResult
    where T : class
{
    public T? Item { get; set; } = null!;

    public bool Success => Errors.Count == 0;

    public Dictionary<string, string> Errors { get; set; } = [];
}

public class EmptyServiceResult : IServiceResult
{
    public bool Success => Errors.Count == 0;

    public Dictionary<string, string> Errors { get; set; } = [];
}
