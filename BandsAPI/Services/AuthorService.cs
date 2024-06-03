using BandsAPI.Api.Models.Authors;
using BandsAPI.Utilities.Interfaces;
using BandsAPI.Data;
using Microsoft.EntityFrameworkCore;
using BandsAPI.Api.Models.Songs;
using BandsAPI.Data.Entities;
using BandsAPI.Utilities.Helpers;
using BandsAPI.Utilities;
using BandsAPI.Api.Services.Interfaces;

namespace BandsAPI.Api.Services;
public class AuthorService(IAppMapper mapper, AppDbContext context) : IAuthorService
{
    private readonly IAppMapper mapper = mapper;
    private readonly AppDbContext context = context;

    public async Task<IEnumerable<AuthorDetail>?> GetListAsync()
    {
        var dbEntities = await context.Authors.ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<IEnumerable<AuthorDetail>?> GetByNameAsync(string name)
    {
        var dbEntities = await context.Set<Author>().Where(x => x.Name.Equals(name)).ToListAsync();
        return dbEntities.Select(mapper.ToDetail);
    }
    public async Task<AuthorDetail?> GetAsync(Guid id)
    {
        var dbEntity = await context.Authors.FirstOrDefaultAsync(x => x.Id.Equals(id));
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
        if (source.Description != null)
        {
            var descriptionLengthCheck = source.Description.Length < 500;
            if (!descriptionLengthCheck)
            {
                result.AddError(nameof(source.Description), "too-long");
            }
        }
        if (result.Success)
        {
            context.Authors.Add(newEntity);
            await context.SaveChangesAsync();
        }
        return result;
    }
    public async Task<ServiceResult<AuthorDetail>> UpdateAsync(AuthorUpdate source, Author target)
    {
        var result = ResultHelper.Create<AuthorDetail>();
        mapper.ApplyUpdate(source, target);
        var uniqueNameCheck = context.Set<Author>().Any(x => x.Name.Equals(source.Name));
        if (!uniqueNameCheck)
        {
            result.AddError(nameof(source.Name), "not-unique");
        }
        if (source.Description != null)
        {
            var descriptionLengthCheck = source.Description.Length < 500;
            if (!descriptionLengthCheck)
            {
                result.AddError(nameof(source.Description), "too-long");
            }
        }
        if (target == null) result.AddError(nameof(target.Id), "not-found");
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
        var dbEntity = context.Authors.FirstOrDefault(x => x.Id.Equals(id));
        if (dbEntity == null) result.AddError(nameof(id), "not-found");
        if (result.Success)
        {
            context.Authors.Remove(dbEntity!);
            await context.SaveChangesAsync();
        }
        return result;
    }
}
