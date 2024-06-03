using BandsAPI.Api.Models.Authors;
using BandsAPI.Data.Entities;
using BandsAPI.Utilities;

namespace BandsAPI.Api.Services.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDetail>?> GetListAsync();
    Task<IEnumerable<AuthorDetail>?> GetByNameAsync(string name);
    Task<AuthorDetail?> GetAsync(Guid id);
    Task<ServiceResult<AuthorDetail>> CreateAsync(AuthorCreate source);
    Task<ServiceResult<AuthorDetail>> UpdateAsync(AuthorUpdate source, Author target);
    Task<EmptyServiceResult> DeleteAsync(Guid id);
}
