using BandsAPI.Api.Models.Songs;
using BandsAPI.Api.Services;
using BandsAPI.Api.Utilities;
using BandsAPI.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BandsAPI.Api.Controllers;
[ApiController]
[Route("BandsAPI/SongsController/")]
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

    [HttpGet("api/v1/Songs/GetSongs")]
    public async Task<ActionResult<IEnumerable<SongDetail>>> GetAllSongsAsync()
    {
        return Ok(await songService.GetAllAsync());
    }
    [HttpGet("api/v1/Songs/GetSong/{id}")]
    public async Task<ActionResult<SongDetail>> GetSongByIdAsync([FromRoute] Guid id)
    {
        var targetDetail = await songService.GetByIdAsync(id);
        return targetDetail != null ? Ok(targetDetail) : NotFound();
    }
    [HttpPost("api/v1/Songs/CreateSong")]
    public async Task<ActionResult<SongDetail>> CreateSongAsync([FromBody] SongCreate source)
    {
        var dbEntityDetail = await songService.CreateAsync(source);
        return dbEntityDetail != null ? Ok(dbEntityDetail) : NotFound();
    }
    [HttpPatch("api/v1/Songs/UpdateSong/{id}")]
    public async Task<ActionResult<SongDetail>> UpdateSongAsync(
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
    [HttpDelete("api/v1/Songs/DeleteSong/{id}")]
    public async Task<IActionResult> DeleteSongAsync([FromRoute] Guid id)
    {
        var result = await songService.DeleteByIdAsync(id);
        return result == true ? NoContent() : NotFound();
    }
}
