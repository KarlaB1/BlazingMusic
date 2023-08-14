using BlazingMusic.Shared.DataContexts;
using BlazingMusic.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingMusic.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly SQLDBContext _dbContext; // Reemplaza "YourDbContext" con tu contexto real

        public MusicController(SQLDBContext context) // Reemplaza "YourDbContext" con tu contexto real
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("GetMusics")]
        public async Task<IActionResult> GetMusics()
        {
            try
            {
                var musics = await _dbContext.Musics.ToListAsync();
                return Ok(musics);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetMusic/{id}")]
        public async Task<IActionResult> GetMusic(int id)
        {
            try
            {
                var music = await _dbContext.Musics.FindAsync(id);
                if (music == null)
                {
                    return NotFound();
                }
                return Ok(music);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateMusic")]
        public async Task<IActionResult> CreateMusic(Music music)
        {
            try
            {
                if (music != null)
                {
                    _dbContext.Add(music);
                    await _dbContext.SaveChangesAsync();
                    return Ok("Music created successfully!");
                }
                return BadRequest("Invalid music data.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateMusic")]
        public async Task<IActionResult> UpdateMusic(Music music)
        {
            _dbContext.Entry(music).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Music updated successfully!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicExists(music.MusicId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteMusic/{id}")]
        public async Task<IActionResult> DeleteMusic(int id)
        {
            var music = await _dbContext.Musics.FindAsync(id);
            if (music == null)
            {
                return NotFound();
            }

            _dbContext.Musics.Remove(music);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool MusicExists(int id)
        {
            return _dbContext.Musics.Any(e => e.MusicId == id);
        }
    }
}
