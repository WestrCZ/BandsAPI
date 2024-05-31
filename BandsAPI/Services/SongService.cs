using BandsAPI.Api.Models.Songs;
using BandsAPI.Utilities.Interfaces;
using BandsAPI.Data;
using BandsAPI.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandsAPI.Utilities.Helpers;
using BandsAPI.Utilities;

namespace BandsAPI.Api.Services;
public class SongService
{
    private readonly AppDbContext context;
    private readonly IAppMapper mapper;
    public SongService(AppDbContext context, IAppMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<SongDetail?> GetAsync(Guid id)
    {
        var dbEntity = await context.Songs.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        return dbEntity != null ? mapper.ToDetail(dbEntity) : null;
    }
    public async Task<IEnumerable<SongDetail>?> GetListAsync()
    {
        var dbEntities = await context.Songs.Include(x => x.Author).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<IEnumerable<SongDetail>?> GetListByAuthorAsync(Guid authorId)
    {
        var dbEntities = await context.Songs.Include(x=> x.Author).Where(x=> x.AuthorId == authorId).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<ServiceResult<SongDetail>> CreateAsync(SongCreate source)
    {
        var response = ResultHelper.Create<SongDetail>();
        var newEntity = mapper.FromCreate(source);
        context.Songs.Add(newEntity);
        await context.SaveChangesAsync();
        response.Item = mapper.ToDetail(newEntity);
        var uniqueNameCheck = context.Set<Song>().Any(x => x.Name == source.Name);
        if (!uniqueNameCheck)
        {
            response.AddError(nameof(source.Name), "Name must be unique");
        }
        return response;
    }
    public async Task<SongDetail?> UpdateAsync(SongUpdate source, Song target)
    {
        mapper.ApplyUpdate(source, target);
        await context.SaveChangesAsync();
        return await GetAsync(target.Id);
    }
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var dbEntity = await context.Songs.FirstOrDefaultAsync(x => x.Id == id);
        if (dbEntity == null) return new NotFoundResult();
        context.Songs.Remove(dbEntity);
        await context.SaveChangesAsync();
        return new OkResult();
    }
}
