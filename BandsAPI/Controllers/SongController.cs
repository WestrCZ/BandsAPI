using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Services;
using BandsAPI.Api.Utilities;
using BandsAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandsAPI.Api.Controllers;
[ApiController]
[Route("api/SongsController/")]
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
    [HttpGet("GetListByAuthor/{authorId}")]
    public async Task<ActionResult<IEnumerable<SongDetail>>> GetList ([FromRoute] Guid authorId)
    {
        var result = await songService.GetListByAuthorAsync(authorId);
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
        var dbEntityDetail = await songService.CreateAsync(source);
        if (!ModelState.IsValid) { return ValidationProblem(ModelState); }
        return dbEntityDetail != null ? Ok(dbEntityDetail) : NotFound();
    }
    [HttpPatch("Update/{id}")]
    public async Task<ActionResult<SongDetail>> Update (
        [FromBody] JsonPatchDocument<SongUpdate> patch,
        [FromRoute] Guid id)
    {
        var dbEntity = await context.Songs.FirstOrDefaultAsync(x => x.Id == id);
        if (dbEntity == null) return NotFound();
        var entityToUpdate = mapper.ToUpdate(dbEntity);
        patch.ApplyTo(entityToUpdate);
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        return Ok(await songService.UpdateAsync(entityToUpdate, dbEntity));
    }
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteSongAsync([FromRoute] Guid id)
    {
        var result = await songService.DeleteAsync(id);
        return result == new OkResult() ? NoContent() : NotFound();
    }
}
