using BanqueTardi.Models;

namespace BanqueTardi.MVC.Interface
{
    public interface ICarteCreditServices
    {
        Task<List<CarteCredit>> ObtenirTout();
        Task<CarteCredit> Obtenir(int id);
        Task Ajouter(CarteCredit carteCredit);
        Task Supprimer(int id);
        Task Modifier(CarteCredit carteCredit);
    }
}
