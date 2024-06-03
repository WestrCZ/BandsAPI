namespace BandsAPI.Utilities.Helpers;
public static class ApiListResultHelper
{
    public static ApiListResult<T> Create<T>(IEnumerable<T> list)
    where T : class
    {
        return new ApiListResult<T>(list);
    }
}
