using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionInterets.Models
{
    public class Interet
    {
        public int CompteID { get; set; }

        public double Solde { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public double Taux { get; set; }

        public double MontantInteret { get; set; }
    }
}
