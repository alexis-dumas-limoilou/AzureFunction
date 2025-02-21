using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanqueTardi.Models
{
    public class Operation
    {
        [Key]
        public int OperationId { get; set; }

        [DisplayName("Compte")]
        public int TypeCompteID { get; set; }

        [DisplayName("Compte")]
        public int CompteId { get; set; }

        public Compte? Compte { get; set; }

        [Display(Name = "Date Operation")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOperation { get; set; } = DateTime.Now;

        [Range(0, double.MaxValue, ErrorMessage = "L'opération ne peux pas avoir une valeur négatif")]
        [RegularExpression(@"^\d*$|(?=^.*$)^\d+\,\d{0,2}$")]
        public decimal Montant { get; set; }

        [Required(ErrorMessage = "Une description de l'opération est obligatoire")]
        public string Libelle { get; set; } = "";

        [Display(Name = "Type d'Opération")]
        public string TypeOperation { get; set; } = "";
    }
}
