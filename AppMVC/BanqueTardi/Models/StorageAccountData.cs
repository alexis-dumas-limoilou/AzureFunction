namespace BanqueTardi.MVC.Models
{
    public class StorageAccountData
    {
        public string? Id { get; set; }
        public string? Value { get; set; }
        public DateTimeOffset? DateAjout { get; set; }
        public DateTimeOffset? DateExpiration { get; set; }
    }
}
