using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BanqueTardi.Models
{
    public class CarteCredit
    {

        public string? Numero {  get; set; }

        [JsonIgnore]
        public int IdClient { get; set; }

        public string? NomDemandeur {  get; set; }

        public string TypeCarte { get; set; }

        public int LimiteCredit { get; set;}
    }
}
