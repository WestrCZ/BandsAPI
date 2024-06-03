using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Services;
using BandsAPI.Utilities;
using BandsAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandsAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]/")]
public class SongController : ControllerBase
{
    private readonly SongService songService;
    private readonly AppDbContext context;
    private readonly AppMapper mapper;

    public SongController(SongService songService, AppDbContext context, AppMapper mapper)
    {
        this.songService = songService;
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet("GetList")]
    public async Task<ActionResult<IEnumerable<SongDetail>>> GetList ()
    {
        var result = await songService.GetListAsync();
        if (!result!.Any())
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpGet("GetByAuthor/{authorId}")]
    public async Task<ActionResult<IEnumerable<SongDetail>>> GetByAuthor ([FromRoute] Guid authorId)
    {
        var result = await songService.GetByAuthorAsync(authorId);
        if (!result!.Any())
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpGet("Get/{id}")]
    public async Task<ActionResult<SongDetail>> Get ([FromRoute] Guid id)
    {
        var targetDetail = await songService.GetAsync(id);
        return targetDetail != null ? Ok(targetDetail) : NotFound();
    }
    [HttpPost("Create")]
    public async Task<ActionResult<SongDetail>> Create ([FromBody] SongCreate source)
    {
        var result = await songService.CreateAsync(source);
        ModelState.AddAllErrors(result);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
        return Ok(result.Item);
    }
    [HttpPatch("Update/{id}")]
    public async Task<ActionResult<SongDetail>> Update (
        [FromBody] JsonPatchDocument<SongUpdate> patch,
        [FromRoute] Guid id)
    {
        var dbEntity = mapper.FromDetail(await songService.GetAsync(id));
        if (dbEntity == null) return NotFound();
        var targetUpdateModel = mapper.ToUpdate(dbEntity);
        patch.ApplyTo(targetUpdateModel);
        var result = await songService.UpdateAsync(targetUpdateModel, dbEntity);
        ModelState.AddAllErrors(result);
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return Ok(result.Item);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteSongAsync([FromRoute] Guid id)
    {
        var result = await songService.DeleteAsync(id);
        ModelState.AddAllErrors(result);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
        return NoContent();
    }
}
