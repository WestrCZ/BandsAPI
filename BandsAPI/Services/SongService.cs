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
    private readonly AppDbContext context = context;
    private readonly IAppMapper mapper = mapper;

    public async Task<SongDetail?> GetAsync(Guid id)
    {
        var dbEntity = await context.Songs.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id.Equals(id));
        return dbEntity != null ? mapper.ToDetail(dbEntity) : null;
    }
    public async Task<IEnumerable<SongDetail>?> GetListAsync()
    {
        var dbEntities = await context.Songs.Include(x => x.Author).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<IEnumerable<SongDetail>?> GetByAuthorAsync(Guid id)
    {
        var dbEntities = await context.Songs.Include(x=> x.Author).Where(x => x.AuthorId.Equals(id)).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<IEnumerable<SongDetail>?> GetByNameAsync(string name)
    {
        var dbEntities = await context.Set<Song>().Include(x => x.Author).Where(x => x.Name.Equals(name)).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<ServiceResult<SongDetail>> CreateAsync(SongCreate source)
    {
        var result = ResultHelper.Create<SongDetail>();
        var newEntity = mapper.FromCreate(source);
        context.Songs.Add(newEntity);
        await context.SaveChangesAsync();
        result.Item = mapper.ToDetail(newEntity);
        var uniqueNameCheck = context.Set<Song>().Any(x => x.Name.Equals(source.Name));
        return result;
    }
    public async Task<ServiceResult<SongDetail>> UpdateAsync(SongUpdate source, Song target)
    {
        var result = ResultHelper.Create<SongDetail>();
        if (target == null) { result.AddError(nameof(target.Id), "not-found");}
        mapper.ApplyUpdate(source, target!);
        if (result.Success)
        {
            await context.SaveChangesAsync();
        }
        result.Item = await GetAsync(target!.Id);
        return result;
    }
    public async Task<EmptyServiceResult> DeleteAsync(Guid id)
    {
        var result = ResultHelper.Empty();
        var dbEntity = await context.Songs.FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (dbEntity == null) { result.AddError(nameof(id), "not-found"); }
        if (result.Success)
        {
            context.Songs.Remove(dbEntity!);
            await context.SaveChangesAsync();
        }
        return result;
    }
}
