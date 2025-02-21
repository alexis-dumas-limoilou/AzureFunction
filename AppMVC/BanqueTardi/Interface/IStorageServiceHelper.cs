using BanqueTardi.MVC.Models;

namespace BanqueTardi.MVC.Interface
{
    public interface IStorageServiceHelper
    {
        Task<IEnumerable<StorageAccountData>> ObtenirMessagesDansQueue(string nomQueue);

        Task EnregistrerMessage(string message, string nomQueue);

        Task<IEnumerable<string>> ObtenirQueues();
    }
}
