using BanqueTardi.Models;
using BanqueTardi.MVC.Interface;

namespace BanqueTardi.MVC.Services
{
    public class CarteCreditServiceProxy : ICarteCreditServices
    {
        private readonly HttpClient _httpClient;

        private const string _carteCreditApiUrl = "api/carteCredits/";

        public CarteCreditServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CarteCredit>> ObtenirTout()
        {
            var response = await _httpClient.GetAsync(_carteCreditApiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CarteCredit>>();

        }

        public async Task<CarteCredit> Obtenir(int id)
        {
            var response = await _httpClient.GetAsync(_carteCreditApiUrl + id);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CarteCredit>();
           
        }

        public async Task Ajouter(CarteCredit carteCredit)
        {
            var response = await _httpClient.PostAsJsonAsync(_carteCreditApiUrl, carteCredit);

            response.EnsureSuccessStatusCode();
        }

        public async Task Supprimer(int id)
        {
            var response = await _httpClient.DeleteAsync(_carteCreditApiUrl + id);

            response.EnsureSuccessStatusCode();
        }

        public async Task Modifier(CarteCredit carteCredit)
        {
            var response = await _httpClient.PutAsJsonAsync(_carteCreditApiUrl + carteCredit.Numero, carteCredit);

            response.EnsureSuccessStatusCode();
        }
    }
}
