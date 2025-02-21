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
using BanqueTardi.MVC.Services;

namespace BanqueTardi.Controllers
{
    public class OperationsController : Controller
    {
        private readonly ClientContext _context;
        private readonly List<string> TypeOperations = new() { "Débit", "Crédit" };
        private readonly IStorageServiceHelper _storageServiceHelper;

        public OperationsController(ClientContext context, IStorageServiceHelper storageServiceHelper)
        {
            _context = context;
            _storageServiceHelper = storageServiceHelper;
        }

        // GET: Operation/Historique
        public async Task<IActionResult> Historique(int? id, string? filtre) 
        {
            if (id == null || _context.Operations == null)
            {
                return NotFound();
            }

            ViewBag.Code = id;
            Compte? compte = await TrouverCompteAsync(id, false);
            if(compte == null)
            {
                return NotFound();
            }

            ViewBag.Id = compte.ClientId;
            IEnumerable<Operation> Operations = compte.Operations!;

            if (filtre != null)
                Operations = Operations.Where(n => n.TypeOperation.Contains(filtre, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return View(Operations);
        }

        // GET: Operations/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.TypeOperations = new SelectList(TypeOperations);
            ViewBag.CodeCourt = id;
            Compte? compte = await TrouverCompteAsync(id, false);
            if(compte == null)
            {
                return NotFound();
            }

            ViewBag.Id = compte.ClientId;
            if (ViewData["TypeOperations"] == null || id is null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: Operations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("OperationId,Montant,Libelle,TypeOperation")] Operation operation)
        {
            if (!TypeOperations.Contains(operation.TypeOperation))
            {
                ModelState.AddModelError("TypeOperation", "Le type d'operation n'est pas valide.");
            }

            if (ModelState.IsValid)
            {
                Compte? compte = await TrouverCompteAsync(id);
                if(compte == null)
                {
                    return NotFound();
                }
                
                operation.TypeCompteID = compte.TypeCompteID;
                operation.CompteId = compte.CompteId;
                operation.DateOperation = DateTime.Now;

                if (compte.Solde < operation.Montant && operation.TypeOperation=="Débit")   
                {
                    if (operation.TypeCompteID == 10)
                    {
                        compte.Solde -= 10;
                        compte.Client!.NbDecouverts++;
                        Operation decouvert = new() {CompteId = compte.CompteId, TypeCompteID = compte.TypeCompteID,  Montant = 10m, Libelle = "Découvert", TypeOperation = "Débit", DateOperation=DateTime.Now };
                        _context.Add(decouvert);
                        string messageQueue = $"Découvert sur le compte bancaire {compte.CompteId}, du client {compte.Client.Nom} d'un montant {10m}$";
                        _storageServiceHelper.EnregistrerMessage(messageQueue, "queuedecouvert");
                    }
                    else if (operation.TypeCompteID != 10)
                    {
                        ModelState.AddModelError("Montant", "Vous ne pouvez avoir de découvert sur ce type de compte.");
                        ViewBag.TypeOperations = new SelectList(TypeOperations);
                        ViewBag.CodeCourt = id;
                        return View(operation);
                    }
                }

                //Mise à jour du solde
                if (operation.TypeOperation == "Débit")
                    compte.Solde -= operation.Montant;
                else 
                    compte.Solde += operation.Montant;

                _context.Update(compte);
                _context.Add(operation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Gerer", "Clients", new { id = compte!.ClientId });
            }

            ViewBag.TypeOperations = new SelectList(TypeOperations);
            ViewBag.CodeCourt = id;
            return View(operation);
        }     

        private async Task<bool> OperationExists(int id)
        {
          return await _context.Operations.AnyAsync(e => e.OperationId == id);
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
                .Include(c => c.Operations)
                .Include(c => c.Client)
                .FirstOrDefaultAsync();
            } else
            {
                compte = await _context.Comptes
                .Where(c => c.TypeCompteID == type)
                .Where(c => c.CompteId == compteId)
                .Include(c => c.Operations)
                .Include(c => c.Client)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync();
            }
            return compte;
        }
    }
}
