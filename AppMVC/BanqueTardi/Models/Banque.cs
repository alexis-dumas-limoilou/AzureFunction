using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BanqueTardi.Models
{
    public class Banque
    {
        public int ID { get; set; }

        public string Nom { get; set; }

        [Required(ErrorMessage = "Le code de Transit est obligatoire")]
        [MaxLength(5, ErrorMessage = "Le code de Transit ne peut pas dépasser 5 caractères")]
        [DisplayName("Code de Transit")]
        public int TransitID { get; set; } 

        [Required(ErrorMessage = "Le numéro d'institution est obligatoire")]
        [MaxLength(3)]
        [DisplayName("Numéro d'institution")]
        public int InstitutionID { get; set; }
    }
}
