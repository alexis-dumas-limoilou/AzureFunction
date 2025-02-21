namespace Assurances.API.Models
{
    public class ContratAssurance
    {
        public int Id { get; set; }
        public string CodePartenaire { get; set; }
        public int IdClient { get; set; }
        public string NomDemandeur { get; set; }
        public string SexeDemandeur { get; set; }
        public DateTime DateNaissance { get; set; }
        public decimal Montant { get; set; }
        public bool EstFumeur { get; set; }
        public bool EstDiabetique { get; set; }
        public bool EstHypertendu { get; set; }
        public bool EstPhysiquementActif { get; set; }

    }
}
