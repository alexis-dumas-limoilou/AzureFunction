using BanqueTardi.Data;
using BanqueTardi.Models;
using BanqueTardi.MVC.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BanqueTardi.MVC.Controllers
{
    public class CarteCreditController : Controller
    {
        private readonly ICarteCreditServices _carteCreditService;
        private readonly ClientContext _context;

        public CarteCreditController(ICarteCreditServices carteCreditService,
            ClientContext context)
    {
        _carteCreditService = carteCreditService;
        _context = context;
    }

    // GET: CarteCreditController
    public async Task<IActionResult> Index()
    {
        var cartes = await _carteCreditService.ObtenirTout();
        return View(cartes);
    }

   

    // GET: CarteCreditController/Create
    public IActionResult Create()
    {
            PopulateClientsDropDownList();
            return View();
    }

    // POST: CarteCreditController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarteCredit carteCredit)
    {
        if (ModelState.IsValid)
        {
                Client client = await _context.Clients.FindAsync(carteCredit.IdClient);

                carteCredit.NomDemandeur = client.Prenom + " " + client.Nom;

                await _carteCreditService.Ajouter(carteCredit);
                return RedirectToAction(nameof(Index));
            
        }
            PopulateClientsDropDownList();
            return View(carteCredit);
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
