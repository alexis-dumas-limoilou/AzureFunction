using CalculInterets.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalculInterets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculCreditController : ControllerBase
    {

        // POST api/<CalculCreditController>
        [HttpPost]
        public List<Interet> Post([FromBody] List<Interet> interets)
        {
            foreach (var interet in interets)
            {

                int nombreMois = ((interet.DateFin.Year - interet.DateDebut.Year) * 12) + interet.DateFin.Month - interet.DateDebut.Month;

                double tauxMensuel = (interet.Taux/100) / 12;
                double montantFinal = interet.Solde * Math.Pow(1 + tauxMensuel, nombreMois);

                interet.MontantInteret = Math.Round(montantFinal - interet.Solde, 2);
            }

            return interets;
        }

    }
}
