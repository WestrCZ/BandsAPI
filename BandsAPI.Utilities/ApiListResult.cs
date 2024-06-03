namespace BandsAPI.Utilities;
public class ApiListResult<T>
    where T : class
{
    public IEnumerable<T> list { get; }

    public ApiListResult(IEnumerable<T> list)
    {
        this.list = list;
    }
}
