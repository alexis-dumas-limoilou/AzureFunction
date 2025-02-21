using System.Net;
using AzFunctionInterets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzFunctionInterets
{
    public class HttpInterets
    {
        private readonly ILogger _logger;
        private static string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

        public HttpInterets(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpInterets>();
        }

        [Function("FunctionInterets")]
        public async Task<OutputType> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext executionContext)
        {
            _logger.LogInformation("C# HTTP trigger function 'FunctionInterets' started.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<Interet> interets;

            try
            {
                interets = JsonConvert.DeserializeObject<List<Interet>>(requestBody);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deserializing: {ex.Message}");
                return new OutputType()
                {
                    Interet = null,
                    HttpResponse = req.CreateResponse(HttpStatusCode.BadRequest)
                };
            }

            // Calcul interet
            foreach (var interet in interets)
            {

                int nombreMois = ((interet.DateFin.Year - interet.DateDebut.Year) * 12) + interet.DateFin.Month - interet.DateDebut.Month;

                double tauxMensuel = (interet.Taux / 100) / 12;
                double montantFinal = interet.Solde * Math.Pow(1 + tauxMensuel, nombreMois);

                interet.MontantInteret = Math.Round(montantFinal - interet.Solde, 2);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(JsonConvert.SerializeObject(interets));

            return new OutputType()
            {
                Interet = interets,
                HttpResponse = response
            };
        }
    }
}
