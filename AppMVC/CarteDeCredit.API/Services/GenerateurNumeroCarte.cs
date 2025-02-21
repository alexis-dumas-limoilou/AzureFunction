using CarteDeCredit.API.Interfaces;

namespace CarteDeCredit.API.Services
{
    public class GenerateurNumeroCarte : IGenerateurNumeroCarte
    {
        public string GenererNumeroCarte(string typeCarte)
        {
            Random random = new();
            string numeroCarte;

            if (typeCarte == "VISA")
            {
                numeroCarte = "4"; 
                                   
                for (int i = 1; i < 16; i++)
                {
                    numeroCarte += random.Next(0, 10).ToString();
                }
            }
            else if (typeCarte == "Mastercard")
            {
                // Générer le préfixe Mastercard (51 à 55)
                int[] prefixesMastercard = { 51, 52, 53, 54, 55 };
                int prefixIndex = random.Next(0, prefixesMastercard.Length);
                numeroCarte = prefixesMastercard[prefixIndex].ToString();

                
                for (int i = numeroCarte.Length; i < 16; i++)
                {
                    numeroCarte += random.Next(0, 10).ToString();
                }
            }
            else
            {
                throw new ArgumentException("Le type de carte doit être 'VISA' ou 'Mastercard'.");
            }

            return numeroCarte;
        }
    }
}
