using BanqueTardi.Models;

namespace BanqueTardi.MVC.Interface
{
    public interface IAssurancesService
    {
        Task<List<ContratAssurance>> ObtenirTout();
        Task<ContratAssurance> Obtenir(int id);
        Task Ajouter(ContratAssurance contratAssurance);
        Task Supprimer(int id);
        Task Modifier(ContratAssurance contratAssurance);
    }
}
