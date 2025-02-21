using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanqueTardi.Data;
using BanqueTardi.Models;

namespace BanqueTardi.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ClientContext _context;

        public ClientsController(ClientContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
              return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,DateNaissance,Adresse,CodePostale,Telephone,NomParent,TelephoneParent")] Client client)
        {
            if (client.DateNaissance.AddYears(15) > DateTime.Now)
            {
                ModelState.AddModelError("DateNaissance", "Vous être trop jeune pour avoir un compte de banque.");
            }
            else if(client.DateNaissance.AddYears(18) >= DateTime.Now)
            {
                if(client.NomParent == null || client.TelephoneParent == null)
                {
                    ModelState.AddModelError("DateNaissance", "Une personne de moins de 18 ans doit avoir l'autorisation d'un parent.");
                    
                    if (client.NomParent == null)
                    {
                        ModelState.AddModelError("NomParent", "Le nom d'un parent est obligatoire.");
                    }
                    if (client.TelephoneParent == null)
                    {
                        ModelState.AddModelError("TelephoneParent", "Le téléphone du parent est obligatoire.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "Home");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(int id, [Bind("ID,NbDecouverts,Nom,Prenom,DateNaissance,Adresse,CodePostale,Telephone,NomParent,TelephoneParent")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Gerer), new {id = client.ID });
            }

            return View(client);
        }


        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'ClientContext.Clients'  is null.");
            }

            var client = await _context.Clients
                .Include(c => c.Comptes!).ThenInclude(c => c.Banque)
                .Include(c => c.Comptes!).ThenInclude(c => c.TypeCompte)
                .Include(c => c.Comptes!).ThenInclude(c => c.Operations!.OrderByDescending(o => o.OperationId).Take(10))
                .FirstOrDefaultAsync(m => m.ID == id);

            if (client != null && !client!.Comptes!.Any())
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home");
        }

        private bool ClientExists(int id)
        {
          return _context.Clients.Any(e => e.ID == id);
        }

        // GET: Clients/Gerer/5
        public async Task<IActionResult> Gerer(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Comptes!).ThenInclude(c => c.Banque)
                .Include(c => c.Comptes!).ThenInclude(c => c.TypeCompte)
                .Include(c => c.Comptes!).ThenInclude(c => c.Operations!.OrderByDescending(o => o.OperationId).Take(10))
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }
    }
}
