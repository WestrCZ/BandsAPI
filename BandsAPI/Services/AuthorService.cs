using BandsAPI.Api.Models.Authors;
using BandsAPI.Api.Utilities;
using BandsAPI.Data;
using Microsoft.EntityFrameworkCore;
using BandsAPI.Api.Models.Songs;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using BandsAPI.Data.Entities;

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

    public async Task<IEnumerable<AuthorDetail>?> GetAllAsync()
    {
        var dbEntities = await context.Authors.Include(x => x.Songs).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<AuthorDetail?> GetByIdAsync(Guid id)
    {
        var dbEntity = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        return dbEntity != null ? mapper.ToDetail(dbEntity) : null;
    }
    public async Task<AuthorDetail?> CreateAsync(AuthorCreate source)
    {
        var newEntity = mapper.FromCreate(source);
        context.Authors.Add(newEntity);
        await context.SaveChangesAsync();
        return newEntity != null ? await GetByIdAsync(newEntity.Id) : null;
    }
    public async Task<AuthorDetail?> UpdateAsync(AuthorUpdate source, Author target)
    {
        mapper.ApplyUpdate(source, target);
        await context.SaveChangesAsync();
        return await GetByIdAsync(target.Id);
    }
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var dbEntity = context.Authors.FirstOrDefault(x => x.Id == id);
        if (dbEntity == null) return false;
        context.Authors.Remove(dbEntity);
        await context.SaveChangesAsync();
        return true;
    }
}
