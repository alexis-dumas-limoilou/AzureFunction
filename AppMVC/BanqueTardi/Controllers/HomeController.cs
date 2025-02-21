using BanqueTardi.Data;
using BanqueTardi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BanqueTardi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ClientContext _context;

        public HomeController(ILogger<HomeController> logger, ClientContext context)
        {
            _logger = logger;
            _context= context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? filtre, List<Client> nomClients)
        {
            nomClients = await _context.Clients.ToListAsync();

            if (filtre != null)
                nomClients = nomClients.Where(n => n.Nom.Contains(filtre, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return View(nomClients);
        }

        public async Task<IActionResult> IndexAsync()
        {            
            return View(await _context.Clients.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}