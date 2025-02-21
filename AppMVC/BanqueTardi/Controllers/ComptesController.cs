using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanqueTardi.Data;
using BanqueTardi.Models;
using System.Numerics;
using BanqueTardi.MVC.Interface;

namespace BanqueTardi.Controllers
{
    public class ComptesController : Controller
    {
        private readonly ClientContext _context;

        private readonly ICalculInteretService _calculInteretService;

        public ComptesController(ClientContext context, ICalculInteretService calculInteretService)
        {
            _calculInteretService = calculInteretService;
            _context = context;
        }

        // GET: Comptes
        public async Task<IActionResult> Index()
        {
            var clientContext = _context.Comptes
                .Include(c => c.Banque)
                .Include(c => c.Client)
                .Include(c => c.TypeCompte).AsNoTracking();
            return View(await clientContext.ToListAsync());
        }

        // GET: Comptes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comptes == null)
            {
                return NotFound();
            }

            var compte = await TrouverCompteAsync(id, false);
            if (compte == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // GET: Comptes/Create
        public async Task<IActionResult> Create(int? id)
        {
            PopulerViewBag(id);
            Client? client = await _context.Clients.FirstOrDefaultAsync(m => m.ID == id);
            ViewData["Client"] = client;
            if (ViewData["Client"] == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Comptes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("CompteId,BanqueId,TypeCompteID,ClientId,Solde")] Compte compte)
        {
            if (compte.Solde < 0)
            {
                ModelState.AddModelError("Solde", "Le Solde ne peut pas être un chiffre negatif.");
            }

            if (ModelState.IsValid)
            {
                int compteId = await _context.Comptes.Where(c => c.TypeCompteID == compte.TypeCompteID).OrderBy(o => o.CompteId).CountAsync() + 1;
                compte.CompteId = compteId;
                _context.Add(compte);
                if (compte.Solde > 0)
                {
                    Operation Depot = CreeOperationStandard(compte, "Depot");
                    _context.Add(Depot);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Gerer", "Clients", new { id });
            }

            PopulerViewBag(id);
            Client? client = await _context.Clients.FirstOrDefaultAsync(m => m.ID == id);
            ViewData["Client"] = client;
            if (ViewData["Client"] == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // GET: Comptes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comptes == null)
            {
                return NotFound();
            }

            Compte? compte = await TrouverCompteAsync(id, false);
            ViewBag.ID = compte!.ClientId;
            if (compte == null)
            {
                return NotFound();
            }
            if (compte.Operations != null && compte.Operations.Any())
            {
                ModelState.AddModelError("Solde", "Un compte ayant des opérations ne peut pas être modifié.");
            }

            PopulerViewBag(id);
            return View(compte);
        }

        // POST: Comptes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("CompteId,ClientId,TypeCompteID,BanqueId,Solde")] Compte compte)
        {
            Compte? Original = await TrouverCompteAsync(id, false);
            if (Original != null && Original.Operations != null && Original.Operations.Any())
            {
                ModelState.AddModelError("Solde", "Un compte ayant des opérations ne peut pas être modifié.");
            }

            if (Original!.ClientId != compte.ClientId)
            {
                return NotFound();
            }

            
            if (Original != null && Original.Operations != null && Original.Operations.Any())
            {
                ModelState.AddModelError("Solde", "Un compte ayant des opérations ne peut pas être modifié.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (compte.TypeCompteID == Original!.TypeCompteID)
                    {
                        if(compte.Solde != Original.Solde)
                        {
                            Operation Depot = CreeOperationStandard(compte, "Depot");
                            _context.Add(Depot);
                        }
                        _context.Update(compte);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Gestion du changement de type de compte
                        var last = await _context.Comptes.Where(c => c.TypeCompteID == compte.TypeCompteID).OrderBy(o => o.CompteId).AsNoTracking().LastAsync();
                        compte.CompteId = last.CompteId + 1;
                        _context.Remove(Original);
                        _context.Add(compte);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompteExists(compte.CodeCourt))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return RedirectToAction("Gerer", "Clients", new { id = compte.ClientId });
            }
            ViewBag.ID = Original!.ClientId;
            PopulerViewBag(id);
            return View(compte);
        }

        // GET: Comptes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comptes == null)
            {
                return NotFound();
            }

            var compte = await TrouverCompteAsync(id);
            ViewBag["Id"] = compte!.ClientId;
            if (compte == null)
            {
                return NotFound();
            }

            return View(compte);
        }

        // POST: Comptes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comptes == null)
            {
                return Problem("Entity set 'ClientContext.Comptes'  is null.");
            }

            var compte = await TrouverCompteAsync(id);
            int ClientID = compte!.ClientId;
            if (compte != null && !compte!.Operations!.Any())
            {
                _context.Comptes.Remove(compte);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Gerer", "Clients", new { id = ClientID });
        }

        // GET: Comptes/CalculInterets
        public async Task<IActionResult> CalculInterets()
        {
            return View();
        }

        // POST: Comptes/CalculInterets
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LancerCalculInterets()
        {
            var comptes = await _context.Comptes
                .Include(c => c.Operations)
                .Include(c => c.TypeCompte)
                .Where(c => (c.TypeCompte.Libelle != "Chèque" && c.Solde > 0) || (c.TypeCompte.Libelle == "Chèque" && c.Solde < 0))
                .ToListAsync();

            List<Interet> interets = comptes.Select(compte =>
            {
                // Obtenir la dernière opération d'intérêt ou la première opération si aucun intérêt n'a été calculé
                DateTime dateDebut = compte.Operations
                    .Where(op => op.Libelle == "Intérêt")
                    .OrderByDescending(op => op.DateOperation)
                    .FirstOrDefault()?.DateOperation ?? compte.Operations.Min(op => op.DateOperation);

                return new Interet
                {
                    CompteID = compte.CompteId,
                    Solde = compte.Solde,
                    DateDebut = dateDebut,
                    DateFin = DateTime.Now,
                    Taux = compte.TypeCompte.Libelle == "Cheque" ? compte.TypeCompte.TauxInteretDecouvert : compte.TypeCompte.TauxInteret
                };
            }).ToList();

            
            if(interets.Count == 0)
            {
                return RedirectToAction(nameof(Index),"Home");
            }

            // Envoyer la requête au service web pour calculer les intérêts
            List<Interet> resultats = await _calculInteretService.Calculer(interets);

            // Mise à jour des opérations pour chaque compte avec le montant d'intérêt calculé
            foreach (var interet in resultats.Where(i => i.MontantInteret >0))
            {
                var compte = comptes.FirstOrDefault(c => c.CompteId == interet.CompteID);
                if (compte != null)
                {
                    Operation nouvelleOperation = new Operation
                    {
                        CompteId = compte.CompteId,
                        TypeCompteID = compte.TypeCompteID,
                        Montant = interet.MontantInteret,
                        DateOperation = DateTime.Now,
                        Libelle = "Intérêt",
                        TypeOperation = "Crédit"
                    };
                    if (compte.TypeCompte.Libelle == "Cheque")
                    {
                        nouvelleOperation.TypeOperation = "Débit";
                        compte.Solde -= interet.MontantInteret;
                    }
                    else
                    {
                       compte.Solde += interet.MontantInteret;
                    }

                    _context.Operations.Add(nouvelleOperation);

                    _context.Update(compte);

                    await _context.SaveChangesAsync();
                }
            }


            return RedirectToAction(nameof(Index), "Home");
        }


        private void PopulerViewBag(int? id)
        {
            ViewData["BanqueId"] = new SelectList(_context.Banques, "ID", "Nom");
            ViewData["TypeComptes"] = new SelectList(_context.TypeComptes, "Identifiant", "Libelle");
        }

        private async Task<Compte?> TrouverCompteAsync(int? Code, bool track = true)
        {
            if (Code == null)
            {
                return null;
            }

            int type = int.Parse(Code.ToString()!.Substring(0, 2));
            int compteId = int.Parse(Code.ToString()!.Substring(2));
            Compte? compte = null;
            if (track)
            {
                compte = await _context.Comptes
                .Where(c => c.TypeCompteID == type)
                .Where(c => c.CompteId == compteId)
                .Include(c => c.Banque)
                .Include(c => c.Client)
                .Include(c => c.Operations)
                .Include(c => c.TypeCompte)
                .FirstOrDefaultAsync();
            }
            else
            {
                compte = await _context.Comptes
                .Where(c => c.TypeCompteID == type)
                .Where(c => c.CompteId == compteId)
                .Include(c => c.Banque)
                .Include(c => c.Client)
                .Include(c => c.Operations)
                .Include(c => c.TypeCompte)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync();
            }
            
            return compte;
        }

        private Compte? TrouverCompte(int Code)
        {
            int type = int.Parse(Code.ToString()!.Substring(0, 2));
            int compteId = int.Parse(Code.ToString()!.Substring(2));

            Compte? compte = _context.Comptes
                .Where(c => c.TypeCompteID == type)
                .Where(c => c.CompteId == compteId)
                .FirstOrDefault();

            return compte;
        }

        private Operation CreeOperationStandard(Compte compte, string target)
        {
            Operation output = new() { };
            if(target == "Depot")
            {
                output = new() { TypeCompteID = compte.TypeCompteID, CompteId = compte.CompteId, Montant = compte.Solde, Libelle = "solde d’ouverture", TypeOperation = "Crédit" };
            }

            return output;
        }

        private bool CompteExists(int id)
        {
          return TrouverCompteAsync(id).Result != null;
        }
    }
}
