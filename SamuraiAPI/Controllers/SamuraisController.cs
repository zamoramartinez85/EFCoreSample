using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamuraisController : ControllerBase
    {
        private readonly SamuraiContext _samuraiContext;

        public SamuraisController(SamuraiContext context)
        {
            _samuraiContext = context;
        }

        // GET: api/Samurais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Samurai>>> GetSamurais()
        {
            return await _samuraiContext.Samurais.ToListAsync();
        }

        // GET: api/Samurais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Samurai>> GetSamurai(int id)
        {
            var samurai = await _samuraiContext.Samurais.FindAsync(id);

            if (samurai == null)
            {
                return NotFound();
            }

            return samurai;
        }

        // PUT: api/Samurais/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSamurai(int id, Samurai samurai)
        {
            if (id != samurai.SamuraiId)
            {
                return BadRequest();
            }

            _samuraiContext.Entry(samurai).State = EntityState.Modified;

            try
            {
                await _samuraiContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SamuraiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Samurais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Samurai>> PostSamurai(Samurai samurai)
        {
            _samuraiContext.Samurais.Add(samurai);
            await _samuraiContext.SaveChangesAsync();

            return CreatedAtAction("GetSamurai", new { id = samurai.SamuraiId }, samurai);
        }

        // DELETE: api/Samurais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSamurai(int id)
        {
            var samurai = await _samuraiContext.Samurais.FindAsync(id);
            if (samurai == null)
            {
                return NotFound();
            }

            _samuraiContext.Samurais.Remove(samurai);
            await _samuraiContext.SaveChangesAsync();

            return NoContent();
        }

        private bool SamuraiExists(int id)
        {
            return _samuraiContext.Samurais.Any(e => e.SamuraiId == id);
        }
    }
}
