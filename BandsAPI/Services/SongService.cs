using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Utilities;
using BandsAPI.Data;
using BandsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BandsAPI.Api.Services;
public class SongService
{
    private readonly AppDbContext context;
    private readonly AppMapper mapper;
    public SongService(AppDbContext context, AppMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public async Task<SongDetail?> GetByIdAsync(Guid id)
    {
        var dbEntity = await context.Songs.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        return dbEntity != null ? mapper.ToDetail(dbEntity) : null;
    }

    public async Task<IEnumerable<SongDetail>?> GetAllAsync()
    {
        var dbEntities = await context.Songs.Include(x => x.Author).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }

    public async Task<SongDetail?> CreateAsync(SongCreate source)
    {
        var newEntity = mapper.FromCreate(source);
        context.Songs.Add(newEntity);
        await context.SaveChangesAsync();
        return newEntity != null ? await GetByIdAsync(newEntity.Id) : null;
    }
    public async Task<SongDetail?> UpdateAsync(SongUpdate source, Song target)
    {
        mapper.ApplyUpdate(source, target);
        await context.SaveChangesAsync();
        return await GetByIdAsync(target.Id);
    }
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var dbEntity = await context.Songs.FirstOrDefaultAsync(x => x.Id == id);
        if (dbEntity == null) return false;
        context.Songs.Remove(dbEntity);
        await context.SaveChangesAsync();
        return true;
    }
}
