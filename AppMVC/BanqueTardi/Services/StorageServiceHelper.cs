using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using BanqueTardi.MVC.Interface;
using BanqueTardi.MVC.Models;

namespace BanqueTardi.MVC.Services
{
    public class StorageServiceHelper : IStorageServiceHelper
    {
        private readonly QueueServiceClient _queueServiceClient;
        public StorageServiceHelper(QueueServiceClient queueClient)
        {

            _queueServiceClient = queueClient;

        }

        public async Task<IEnumerable<StorageAccountData>> ObtenirMessagesDansQueue(string nomQueue)
        {
            List<StorageAccountData> storageAccountDatas = new();

            //Obtention d'une queue 
            var queueClient = _queueServiceClient.GetQueueClient(nomQueue);

            //Lecture des messages dans la queue
            var messages = await queueClient.ReceiveMessagesAsync(30);

            foreach (QueueMessage message in messages.Value)
            {
                storageAccountDatas.Add(new StorageAccountData
                {
                    Id = message.MessageId,
                    DateAjout = message.InsertedOn,
                    DateExpiration = message.ExpiresOn,
                    Value = message.Body.ToString()
                });
            }

            return storageAccountDatas;
        }

        public Task EnregistrerMessage(string message, string nomQueue)
        {
            //Obtention d'une queue 
            var queueClient = _queueServiceClient.GetQueueClient(nomQueue);


            //Envoi du message 
            return queueClient.SendMessageAsync(message);
        }

        public async Task<IEnumerable<string>> ObtenirQueues()
        {
            List<string> queues = [];

            //Obtention de toutes les queues dans le compte de stockage
            await foreach (QueueItem queue in _queueServiceClient.GetQueuesAsync())
            {
                queues.Add(queue.Name);
            }

            return queues;
        }
    }
}
