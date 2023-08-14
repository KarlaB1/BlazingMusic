using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazingMusic.Shared.Models;
using BlazingMusic.Shared.DataContexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingMusic.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly SQLDBContext _dbContext;

        public MusicController(SQLDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("GetMusics")]
        public async Task<IList<Music>> GetMusics()
        {
            try
            {
                var data = await _dbContext.Musics.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetMusic/{id}")]
        public async Task<ActionResult<Music>> GetMusic(int id)
        {
            try
            {
                var data = await _dbContext.Musics.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("SaveMusic")]
        public async Task<IActionResult> SaveMusic(Music music)
        {
            try
            {
                if (music != null)
                {
                    _dbContext.Add(music);
                    await _dbContext.SaveChangesAsync();

                    return Ok("Music saved successfully!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return NoContent();
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
            if (_dbContext.Musics == null)
            {
                return NotFound();
            }
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
            return (_dbContext.Musics?.Any(e => e.MusicId == id)).GetValueOrDefault();
        }
    }
}
