using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assurances.API.Data;
using Assurances.API.Models;

namespace Assurances.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratAssurancesController : ControllerBase
    {
        private readonly AssurancesContext _context;

        public ContratAssurancesController(AssurancesContext context)
        {
            _context = context;
        }

        // GET: api/ContratAssurances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContratAssurance>>> GetContratAssurances()
        {
            return await _context.ContratAssurances.ToListAsync();
        }

        // GET: api/ContratAssurances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContratAssurance>> GetContratAssurance(int id)
        {
            var contratAssurance = await _context.ContratAssurances.FindAsync(id);

            if (contratAssurance == null)
            {
                return NotFound();
            }

            return contratAssurance;
        }

        // PUT: api/ContratAssurances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContratAssurance(int id, ContratAssurance contratAssurance)
        {
            if (id != contratAssurance.Id)
            {
                return BadRequest();
            }

            _context.Entry(contratAssurance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratAssuranceExists(id))
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

        // POST: api/ContratAssurances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContratAssurance>> PostContratAssurance(ContratAssurance contratAssurance)
        {
            _context.ContratAssurances.Add(contratAssurance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContratAssurance", new { id = contratAssurance.Id }, contratAssurance);
        }

        // DELETE: api/ContratAssurances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContratAssurance(int id)
        {
            var contratAssurance = await _context.ContratAssurances.FindAsync(id);
            if (contratAssurance == null)
            {
                return NotFound();
            }

            _context.ContratAssurances.Remove(contratAssurance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContratAssuranceExists(int id)
        {
            return _context.ContratAssurances.Any(e => e.Id == id);
        }
    }
}
