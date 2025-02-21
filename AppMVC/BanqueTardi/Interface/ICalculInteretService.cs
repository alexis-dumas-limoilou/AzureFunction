using BanqueTardi.Models;

namespace BanqueTardi.MVC.Interface
{
    public interface ICalculInteretService
    {
        Task<List<Interet>> Calculer(List<Interet> interets);
    }
}
