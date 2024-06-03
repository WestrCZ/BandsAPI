using BandsAPI.Api.Models.Authors;
using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Services;
using BandsAPI.Utilities;
using BandsAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BandsAPI.Api.Services.Interfaces;

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
        var result = await authorService.GetListAsync();
        if (!result!.Any())
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpGet("GetByName")]
    public async Task<ActionResult<IEnumerable<AuthorDetail>>> GetByName(string name)
    {
        var result = await authorService.GetByNameAsync(name);
        if (!result!.Any())
        {
            return NotFound();
        }
        return Ok(result);
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
        var result = await authorService.CreateAsync(source);
        ModelState.AddAllErrors(result);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
        var url = Url.Action(nameof(Get), new { result.Item!.Id }) ?? throw new Exception("failed to generate url");
        return Created(url, result.Item);
    }
    [HttpPatch("Update/{id}")]
    public async Task<ActionResult<AuthorDetail>> Update(
        [FromBody] JsonPatchDocument<AuthorUpdate> patch, 
        [FromRoute] Guid id)
    {
        var target = mapper.FromDetail(await authorService.GetAsync(id));
        if (target == null) return NotFound();
        var targetUpdateModel = mapper.ToUpdate(target);
        patch.ApplyTo(targetUpdateModel);
        var result = await authorService.UpdateAsync(targetUpdateModel, target);
        ModelState.AddAllErrors(result);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState);}
        return Ok(result.Item);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<AuthorDetail>> Delete([FromRoute] Guid id)
    {
        var result = await authorService.DeleteAsync(id);
        ModelState.AddAllErrors(result);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
        return NoContent();
    }
}
