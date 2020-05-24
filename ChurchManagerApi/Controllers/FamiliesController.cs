using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChurchManagerApi.Data;
using ChurchManagerApi.Models;

namespace ChurchManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FamiliesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Families
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Family>>> GetFamily()
        {
            return await _context.Family.ToListAsync();
        }

        // GET: api/Families/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Family>> GetFamily(Guid id)
        {
            var family = await _context.Family.FindAsync(id);

            if (family == null)
            {
                return NotFound();
            }

            return family;
        }

        // PUT: api/Families/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFamily(Guid id, Family family)
        {
            if (id != family.Id)
            {
                return BadRequest();
            }

            _context.Entry(family).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyExists(id))
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

        // POST: api/Families
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Family>> PostFamily(Family family)
        {
            _context.Family.Add(family);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamily", new { id = family.Id }, family);
        }

        // DELETE: api/Families/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Family>> DeleteFamily(Guid id)
        {
            var family = await _context.Family.FindAsync(id);
            if (family == null)
            {
                return NotFound();
            }

            _context.Family.Remove(family);
            await _context.SaveChangesAsync();

            return family;
        }

        private bool FamilyExists(Guid id)
        {
            return _context.Family.Any(e => e.Id == id);
        }
    }
}
