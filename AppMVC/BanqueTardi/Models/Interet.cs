namespace BanqueTardi.Models
{
    public class Interet
    {
        public int CompteID { get; set; }

        public decimal Solde {  get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public double Taux { get; set; }

        public decimal MontantInteret {  get; set; }
    }
}
