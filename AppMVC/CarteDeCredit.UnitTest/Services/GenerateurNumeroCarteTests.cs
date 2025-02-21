
using CarteDeCredit.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteDeCredit.UnitTest.Services
{
    public class GenerateurNumeroCarteTests
    {
        private readonly GenerateurNumeroCarte _generateur;

        public GenerateurNumeroCarteTests()
        {
            _generateur = new GenerateurNumeroCarte();
        }

        [Fact]
        public void GenererNumeroCarte_VISA_ReturnsValidVisaNumber()
        {
            // Arrange
            string typeCarte = "VISA";

            // Act
            string numeroCarte = _generateur.GenererNumeroCarte(typeCarte);

            // Assert
            Assert.NotNull(numeroCarte);
            Assert.Equal(16, numeroCarte.Length); // Vérifie la longueur
            Assert.StartsWith("4", numeroCarte); // Vérifie le préfixe pour VISA
        }

        [Fact]
        public void GenererNumeroCarte_Mastercard_ReturnsValidMastercardNumber()
        {
            // Arrange
            string typeCarte = "Mastercard";
            int[] prefixesMastercard = { 51, 52, 53, 54, 55 };

            // Act
            string numeroCarte = _generateur.GenererNumeroCarte(typeCarte);

            // Assert
            Assert.NotNull(numeroCarte);
            Assert.Equal(16, numeroCarte.Length); // Vérifie la longueur

            // Vérifie si le numéro commence avec un des préfixes valides pour Mastercard
            int prefix = int.Parse(numeroCarte.Substring(0, 2));
            Assert.Contains(prefix, prefixesMastercard);
        }

        [Fact]
        public void GenererNumeroCarte_InvalidCardType_ThrowsArgumentException()
        {
            // Arrange
            string typeCarteInvalide = "AMEX"; // Type de carte invalide

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _generateur.GenererNumeroCarte(typeCarteInvalide));
            Assert.Equal("Le type de carte doit être 'VISA' ou 'Mastercard'.", exception.Message);
        }
    }
}
