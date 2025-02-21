using BanqueTardi.Models;
using BanqueTardi.MVC.Interface;

namespace BanqueTardi.MVC.Services
{
    public class AssurancesServiceProxy : IAssurancesService
    {

        private readonly HttpClient _httpClient;

        private const string _assurancesApiUrl = "api/contratAssurances/";

        public AssurancesServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ContratAssurance>> ObtenirTout()
        {
            var response = await _httpClient.GetAsync(_assurancesApiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ContratAssurance>>();
        }

        public async Task<ContratAssurance> Obtenir(int id)
        {
            var response = await _httpClient.GetAsync(_assurancesApiUrl + id);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ContratAssurance>();
        }

        public async Task Ajouter(ContratAssurance contratAssurance)
        {
            var response = await _httpClient.PostAsJsonAsync(_assurancesApiUrl, contratAssurance);
            response.EnsureSuccessStatusCode();
        }

        public async Task Supprimer(int id)
        {
            var response = await _httpClient.DeleteAsync(_assurancesApiUrl + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task Modifier(ContratAssurance contratAssurance)
        {
            var response = await _httpClient.PutAsJsonAsync(_assurancesApiUrl + contratAssurance.Id, contratAssurance);
            response.EnsureSuccessStatusCode();
        }



    }
}
