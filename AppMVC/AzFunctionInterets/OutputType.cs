using AzFunctionInterets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzFunctionInterets
{
    public class OutputType
    {
        [SqlOutput("dbo.Interets", connectionStringSetting: "SqlConnectionString")]
        public List<Interet> Interet { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
}
