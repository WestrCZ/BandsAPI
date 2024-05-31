using BandsAPI.Api.Models.Authors;
using BandsAPI.Utilities.Interfaces;
using BandsAPI.Data;
using Microsoft.EntityFrameworkCore;
using BandsAPI.Api.Models.Songs;
using BandsAPI.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using BandsAPI.Utilities.Helpers;
using BandsAPI.Utilities;

namespace BandsAPI.Api.Services;
public class AuthorService
{
    private readonly IAppMapper mapper;
    private readonly AppDbContext context;
    public AuthorService(IAppMapper mapper, AppDbContext context)
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
    public async Task<ServiceResult<AuthorDetail>> CreateAsync(AuthorCreate source)
    {
        var result = ResultHelper.Create<AuthorDetail>();
        var newEntity = mapper.FromCreate(source);
        result.Item = mapper.ToDetail(newEntity);
        var uniqueNameCheck = context.Set<Author>().Any(x => x.Name.Equals(source.Name));
        if (!uniqueNameCheck)
        {
            result.AddError(nameof(source.Name), "not-unique");
        }
        if (result.Success)
        {
            context.Authors.Add(newEntity);
            await context.SaveChangesAsync();
        }
        return result;
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
