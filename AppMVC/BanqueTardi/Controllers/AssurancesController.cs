using BanqueTardi.Data;
using BanqueTardi.Models;
using BanqueTardi.MVC.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BanqueTardi.MVC.Controllers
{
    public class AssurancesController : Controller
    {
        private readonly IAssurancesService _assurancesService;
        private readonly ClientContext _context;

        public AssurancesController(IAssurancesService assurancesService,
            ClientContext context)
        {
            _assurancesService = assurancesService;
            _context = context;
        }

        // GET: AssurancesController
        public async Task<IActionResult> Index()
        {
            var contrats = await _assurancesService.ObtenirTout();
            return View(contrats);
        }

       

        // GET: AssurancesController/Create
        public IActionResult Create()
        {

            PopulateClientsDropDownList();
            return View();
        }

        // POST: AssurancesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContratAssurance contratAssurance)
        {
            if (ModelState.IsValid)
            {
                Client client = await _context.Clients.FindAsync(contratAssurance.IdClient);
                contratAssurance.DateNaissance = client.DateNaissance;
                contratAssurance.NomDemandeur = client.Prenom + " " + client.Nom;
                contratAssurance.CodePartenaire = "BANQUE";
                await _assurancesService.Ajouter(contratAssurance);
                return RedirectToAction(nameof(Index));
               
            }
            PopulateClientsDropDownList();
            return View(contratAssurance);
        }

       

        private void PopulateClientsDropDownList()
        {
            ViewData["IdClient"] = new SelectList(
           _context.Clients.Select(c => new
           {
               Identifiant = c.ID,
               PrenomNom = c.Prenom + " " + c.Nom
           }),
            "Identifiant",
            "PrenomNom");
        }


    }
}
