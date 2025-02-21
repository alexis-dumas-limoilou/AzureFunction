using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanqueTardi.Models
{
    public class Compte
    {
        public int CodeCourt
        {
            get { return int.Parse($"{TypeCompteID:D2}{CompteId:D5}"); }
        }

        public string CodeLong
        {
            get 
            {
                if (Banque is null)
                {
                    return $"00000000{CodeCourt:D7}";
                } 
                else
                {
                    return $"{Banque.TransitID:D5}{Banque.InstitutionID:D3}{CodeCourt:D7}";
                }
            }
        }

        //Première Clé composite
        [DisplayName("TypeCompte")]
        public int TypeCompteID { get; set; }

        public TypeCompte? TypeCompte { get; set; }

        //Deuxième clé composite
        public int CompteId { get; set; } = 1;

        [DisplayName("Banque")]
        public int BanqueId { get; set; }

        public Banque? Banque { get; set; }

        [DisplayName("Client")]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [RegularExpression(@"^\d*$|(?=^.*$)^\d+\,\d{0,2}$", ErrorMessage = "Vous ne pouvez pas entrer plus de deux decimal")]
        public decimal Solde { get; set; } = 0;

        public ICollection<Operation>? Operations { get; set; }
    }
}
