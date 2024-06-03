using BandsAPI.Api.Models.Songs;
using BandsAPI.Data.Entities;
using BandsAPI.Utilities;

namespace BandsAPI.Api.Services.Interfaces;

public interface ISongService
{
    Task<SongDetail?> GetAsync(Guid id);
    Task<IEnumerable<SongDetail>?> GetListAsync();
    Task<IEnumerable<SongDetail>?> GetByAuthorAsync(Guid id);
    Task<IEnumerable<SongDetail>?> GetByNameAsync(string name);
    Task<ServiceResult<SongDetail>> CreateAsync(SongCreate source);
    Task<ServiceResult<SongDetail>> UpdateAsync(SongUpdate source, Song target);
    Task<EmptyServiceResult> DeleteAsync(Guid id);

}
