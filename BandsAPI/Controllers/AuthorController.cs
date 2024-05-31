using BandsAPI.Api.Models.Authors;
using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Services;
using BandsAPI.Utilities;
using BandsAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandsAPI.Api.Controllers;
[ApiController]
[Route("api/v1/AuthorController/")]
public class AuthorController : ControllerBase
{
    private readonly AppDbContext context;
    private readonly AppMapper mapper;
    private readonly AuthorService authorService;
    public AuthorController(AppDbContext context, AppMapper mapper, AuthorService authorService)
    {
        this.context = context;
        this.mapper = mapper;
        this.authorService = authorService;
    }

    [HttpGet("GetList")]
    public async Task<ActionResult<IEnumerable<AuthorDetail>>> GetList ()
    {
        return Ok(await authorService.GetListAsync());
    }
    [HttpGet("Get/{id}")]
    public async Task<ActionResult<AuthorDetail>> Get ([FromRoute] Guid id)
    {
        var targetDetail = await authorService.GetAsync(id);
        return targetDetail != null ? Ok(targetDetail) : NotFound();
    }
    [HttpPost("Create")]
    public async Task<ActionResult<AuthorDetail>> Create([FromBody] AuthorCreate source)
    {
        var dbEntityDetail = await authorService.CreateAsync(source);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
        return dbEntityDetail != null ? Ok(dbEntityDetail) : NotFound();
    }
    [HttpPatch("Update/{id}")]
    public async Task<ActionResult<AuthorDetail>> Update(
        [FromBody] JsonPatchDocument<AuthorUpdate> patch, 
        [FromRoute] Guid id)
    {
        var dbEntity = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        if (dbEntity == null) return NotFound();
        var entityToUpdate = mapper.ToUpdate(dbEntity);
        patch.ApplyTo(entityToUpdate);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState);}
        return Ok(await authorService.UpdateAsync(entityToUpdate, dbEntity));
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<AuthorDetail>> Delete([FromRoute] Guid id)
    {
        var result = await authorService.DeleteAsync(id);
        return result == new OkResult() ? NoContent() : NotFound();
    }
}
