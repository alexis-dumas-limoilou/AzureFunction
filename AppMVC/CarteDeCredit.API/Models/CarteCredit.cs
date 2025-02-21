using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarteDeCredit.API.Models
{
    public class CarteCredit
    {

        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public string? Numero {  get; set; }

        public string NomDemandeur {  get; set; }

        public string TypeCarte { get; set; }

        public int LimiteCredit { get; set;}
    }
}
