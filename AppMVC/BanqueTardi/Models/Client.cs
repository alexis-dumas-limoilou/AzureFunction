using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace BanqueTardi.Models
{
    public class Client
    {
        public int ID { get; set; }

        public string CodeClient
        {
            get { return $"CL{ID:00000000}"; }
        }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [MaxLength(150, ErrorMessage = "Le nom ne peut pas dépasser 150 caractères")]
        public string Nom { get; set; } 

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [MaxLength(150, ErrorMessage = "Le prénom ne peut pas dépasser 150 caractères")]
        public string Prenom { get; set; } 

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [Display(Name = "Date Naissance")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateNaissance { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [MaxLength(150, ErrorMessage = "L'adresse ne peut pas dépasser 250 caractères")]
        public string Adresse { get; set; } 

        [DataType(DataType.PostalCode)]
        [Required(ErrorMessage = "Le code postale est obligatoire")]
        public string CodePostale { get; set; }

        public int NbDecouverts { get; set; } = 0;

        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Le format de téléphone doit être de type canadien a 10 chiffres")]
        public string? Telephone { get; set; }

        public string? NomParent { get; set; } = null;

        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Le format de téléphone doit être de type canadien a 10 chiffres")]
        public string? TelephoneParent { get; set; } = null;

        public string FullName { get { return Prenom + " " + Nom; }}

        public ICollection<Compte>? Comptes { get; set; }
    }
}
