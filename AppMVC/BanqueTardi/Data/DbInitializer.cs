using BanqueTardi.Models;

namespace BanqueTardi.Data
{
    public class DbInitializer
    {
        public static void Initialize(ClientContext context)
        {
            //On valide s'il y a des données dans la BD
            if (!context.Clients.Any())
            {
                var clients = new List<Client>()
                {
                    new Client() {Nom="Doe", Prenom="John", DateNaissance=DateTime.Now.AddMonths(-492), Adresse="1625 rue Gagnon", CodePostale="G3E1X8", NomParent="Papa", TelephoneParent="4184445656", Telephone="4184546025"},
                    new Client() {Nom="Doe", Prenom="Jeanne", DateNaissance=DateTime.Now.AddMonths(-299), Adresse="889 rue des mélèzes", CodePostale="G3E1X8", Telephone="4188314665"},
                    new Client() {Nom="Bob", Prenom="Billy", DateNaissance=DateTime.Today.AddYears(-15), Adresse="3 Pole Sud", CodePostale="H0H0N0", NomParent="Richard", TelephoneParent="4108001000"}
                };

                context.AddRange(clients);
                context.SaveChanges();
            }

            if (!context.Banques.Any())
            {
                var banques = new List<Banque>()
                {
                    new Banque(){ Nom="Tardi", TransitID = 234, InstitutionID= 1}
                };

                context.AddRange(banques);
                context.SaveChanges();
            }

            if (!context.TypeComptes.Any())
            {
                var TComptes = new List<TypeCompte>()
                {
                    new TypeCompte(10, "Chèque", 0, 7),
                    new TypeCompte(11, "Épargne", 2, 0),
                    new TypeCompte(16, "Compte de placement garanti", 4, 0)
                };
                context.AddRange(TComptes);
                context.SaveChanges();
            }

            if (!context.Comptes.Any())
            {
                var comptes = new List<Compte>()
                {
                    new Compte() {TypeCompteID = 11, BanqueId = context.Banques.First().ID, ClientId = context.Clients.First().ID},
                    new Compte() {TypeCompteID = 10, BanqueId = context.Banques.First().ID, ClientId = context.Clients.OrderBy(m => m.ID).Last().ID}
                };

                context.AddRange(comptes);
                context.SaveChanges();
            }

            if(!context.Operations.Any())
            {
                var operations = new List<Operation>()
                {
                    new Operation() {CompteId = context.Comptes.First().CompteId, TypeCompteID = context.Comptes.First().TypeCompteID, Montant = 100m, Libelle = "Dépôt initial", TypeOperation = "Crédit", DateOperation = DateTime.Now.AddYears(-1)},
                    new Operation() {CompteId = context.Comptes.OrderBy(m => m.CompteId).Last().CompteId, TypeCompteID = context.Comptes.OrderBy(m => m.CompteId).Last().TypeCompteID, Montant = 100m, Libelle = "Dépôt initial", TypeOperation = "Crédit", DateOperation = DateTime.Now.AddYears(-1)}
                };

                context.AddRange(operations);
                context.SaveChanges();
            }
        }
    }
}
