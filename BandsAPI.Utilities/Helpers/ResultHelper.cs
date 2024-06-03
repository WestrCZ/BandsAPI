using BandsAPI.Utilities;

namespace BandsAPI.Utilities.Helpers;
public static class ResultHelper
{
    public static ServiceResult<T> Create<T>()
        where T : class
        => new();
    public static EmptyServiceResult Empty() => new();
}
