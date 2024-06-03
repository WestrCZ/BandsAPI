using BandsAPI.Api.Models.Songs;
using BandsAPI.Utilities.Interfaces;
using BandsAPI.Data;
using BandsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using BandsAPI.Utilities.Helpers;
using BandsAPI.Utilities;
using BandsAPI.Api.Services.Interfaces;

namespace BandsAPI.Api.Services;
public class SongService(AppDbContext context, IAppMapper mapper) : ISongService
{
    private readonly AppDbContext dbContext = context;
    private readonly IAppMapper mapper = mapper;

    public async Task<SongDetail?> GetAsync(Guid id)
    {
        var dbEntity = await dbContext.Songs.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id.Equals(id));
        return dbEntity != null ? mapper.ToDetail(dbEntity) : null;
    }
    public async Task<IEnumerable<SongDetail>?> GetListAsync()
    {
        var dbEntities = await dbContext.Songs.Include(x => x.Author).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<IEnumerable<SongDetail>?> GetByAuthorAsync(Guid id)
    {
        var dbEntities = await dbContext.Songs.Include(x => x.Author).Where(x => x.AuthorId.Equals(id)).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<IEnumerable<SongDetail>?> GetByNameAsync(string name)
    {
        var dbEntities = await dbContext.Set<Song>().Include(x => x.Author).Where(x => x.Name.Equals(name)).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<ServiceResult<SongDetail>> CreateAsync(SongCreate source)
    {
        var result = ResultHelper.Create<SongDetail>();
        var newEntity = mapper.FromCreate(source);
        if (result.Success)
        {
            dbContext.Songs.Add(newEntity);
            await dbContext.SaveChangesAsync();
        }
        var dbEntity = await dbContext.Set<Song>().Include(x => x.Author).FirstAsync(x => x.Id.Equals(newEntity.Id));
        result.Item = mapper.ToDetail(dbEntity);
        return result;
    }
    public async Task<ServiceResult<SongDetail>> UpdateAsync(SongUpdate source, Song target)
    {
        var result = ResultHelper.Create<SongDetail>();
        if (target == null) { result.AddError(nameof(target.Id), "not-found"); }
        mapper.ApplyUpdate(source, target!);
        if (result.Success)
        {
            await dbContext.SaveChangesAsync();
        }
        result.Item = await GetAsync(target!.Id);
        return result;
    }
    public async Task<EmptyServiceResult> DeleteAsync(Guid id)
    {
        var result = ResultHelper.Empty();
        var dbEntity = await dbContext.Songs.FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (dbEntity == null) { result.AddError(nameof(id), "not-found"); }
        if (result.Success)
        {
            dbContext.Songs.Remove(dbEntity!);
            await dbContext.SaveChangesAsync();
        }
        return result;
    }
}
