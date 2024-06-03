namespace BandsAPI.Utilities.Interfaces;
public interface IApiListResult<T>
 where T : class
{
    public IEnumerable<T> list { get; }
}
