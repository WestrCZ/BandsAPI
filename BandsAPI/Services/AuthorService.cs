using BandsAPI.Api.Models.Authors;
using BandsAPI.Api.Utilities;
using BandsAPI.Data;
using Microsoft.EntityFrameworkCore;
using BandsAPI.Api.Models.Songs;
using BandsAPI.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BandsAPI.Api.Services;
public class AuthorService
{
    private readonly AppMapper mapper;
    private readonly AppDbContext context;
    public AuthorService(AppMapper mapper, AppDbContext context)
    {
        this.mapper = mapper;
        this.context = context;
    }

    public async Task<IEnumerable<AuthorDetail>?> GetListAsync()
    {
        var dbEntities = await context.Authors.ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<AuthorDetail?> GetAsync(Guid id)
    {
        var dbEntity = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        return dbEntity != null ? mapper.ToDetail(dbEntity) : null;
    }
    public async Task<AuthorDetail?> CreateAsync(AuthorCreate source)
    {
        var newEntity = mapper.FromCreate(source);
        context.Authors.Add(newEntity);
        await context.SaveChangesAsync();
        return newEntity != null ? await GetAsync(newEntity.Id) : null;
    }
    public async Task<AuthorDetail?> UpdateAsync(AuthorUpdate source, Author target)
    {
        mapper.ApplyUpdate(source, target);
        await context.SaveChangesAsync();
        return await GetAsync(target.Id);
    }
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var dbEntity = context.Authors.FirstOrDefault(x => x.Id == id);
        if (dbEntity == null) return new NotFoundResult();
        context.Authors.Remove(dbEntity);
        await context.SaveChangesAsync();
        return new OkResult();
    }
}
