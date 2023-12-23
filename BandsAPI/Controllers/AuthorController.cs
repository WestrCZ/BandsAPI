using BandsAPI.Api.Models.Authors;
using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Services;
using BandsAPI.Api.Utilities;
using BandsAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandsAPI.Api.Controllers;
[ApiController]
[Route("BandsAPI/AuthorController/")]
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

    [HttpGet("api/v1/Authors/GetAuthors")]
    public async Task<ActionResult<IEnumerable<AuthorDetail>>> GetAllAuthorsAsync()
    {
        return Ok(await authorService.GetAllAsync());
    }
    [HttpGet("api/v1/Authors/GetAuthor/{id}")]
    public async Task<ActionResult<AuthorDetail>> GetAuthorByIdAsync([FromRoute] Guid id)
    {
        var targetDetail = await authorService.GetByIdAsync(id);
        return targetDetail != null ? Ok(targetDetail) : NotFound();
    }
    [HttpPost("api/v1/Authors/CreateAuthor")]
    public async Task<ActionResult<AuthorDetail>> CreateAuthorAsync([FromBody] AuthorCreate source)
    {
        var dbEntityDetail = await authorService.CreateAsync(source);
        return dbEntityDetail != null ? Ok(dbEntityDetail) : NotFound();
    }
    [HttpPatch("api/v1/Authors/UpdateAuthor/{id}")]
    public async Task<ActionResult<AuthorDetail>> UpdateAuthorAsync(
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
    [HttpDelete("api/v1/Authors/DeleteAuthor/{id}")]
    public async Task<ActionResult<AuthorDetail>> DeleteAuthorAsync([FromRoute] Guid id)
    {
        var result = await authorService.DeleteByIdAsync(id);
        return result == true ? NoContent() : NotFound();
    }
}
