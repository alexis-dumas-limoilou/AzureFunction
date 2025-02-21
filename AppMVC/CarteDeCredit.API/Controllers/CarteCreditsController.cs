using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarteDeCredit.API.Data;
using CarteDeCredit.API.Models;
using CarteDeCredit.API.Interfaces;

namespace CarteDeCredit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarteCreditsController : ControllerBase
    {
        private readonly CarteDeCreditDBContext _context;

        private readonly IGenerateurNumeroCarte _generateurNumeroCarte;

        public CarteCreditsController(CarteDeCreditDBContext context,
            IGenerateurNumeroCarte generateurNumeroCarte)
        {
            _context = context;
            _generateurNumeroCarte = generateurNumeroCarte;
        }

        // GET: api/CarteCredits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarteCredit>>> GetCarteCredits()
        {
            return await _context.CarteCredits.ToListAsync();
        }

        // GET: api/CarteCredits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarteCredit>> GetCarteCredit(string id)
        {
            var carteCredit = await _context.CarteCredits.FindAsync(id);

            if (carteCredit == null)
            {
                return NotFound();
            }

            return carteCredit;
        }

        // PUT: api/CarteCredits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarteCredit(string id, CarteCredit carteCredit)
        {
            if (id != carteCredit.Numero)
            {
                return BadRequest();
            }

            _context.Entry(carteCredit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarteCreditExists(id))
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

        // POST: api/CarteCredits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarteCredit>> PostCarteCredit(CarteCredit carteCredit)
        {
            
            try
            {
                carteCredit.Numero = _generateurNumeroCarte.GenererNumeroCarte(carteCredit.TypeCarte);
                _context.CarteCredits.Add(carteCredit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarteCreditExists(carteCredit.Numero))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarteCredit", new { id = carteCredit.Numero }, carteCredit);
        }

        // DELETE: api/CarteCredits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarteCredit(string id)
        {
            var carteCredit = await _context.CarteCredits.FindAsync(id);
            if (carteCredit == null)
            {
                return NotFound();
            }

            _context.CarteCredits.Remove(carteCredit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarteCreditExists(string id)
        {
            return _context.CarteCredits.Any(e => e.Numero == id);
        }
    }
}
