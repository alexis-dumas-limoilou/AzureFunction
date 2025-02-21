using BanqueTardi.Models;
using BanqueTardi.MVC.Interface;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BanqueTardi.MVC.Services
{
    public class CalculInteretServiceProxy : ICalculInteretService
    {
        private readonly HttpClient _httpClient;

        private const string _calculInteretApiUrl = "api/FunctionInterets";

        public CalculInteretServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      
        public async Task<List<Interet>> Calculer(List<Interet> interets)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(), new JsonDateTimeConverter() } // Ensures correct date format
            };

            string jsonContent = JsonSerializer.Serialize(interets, jsonOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_calculInteretApiUrl, httpContent);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Interet>>();
        }
    }

    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")); // Ensure proper formatting
        }
    }
}
