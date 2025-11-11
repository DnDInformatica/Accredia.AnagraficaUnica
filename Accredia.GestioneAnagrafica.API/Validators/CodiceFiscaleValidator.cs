using System;
using System.Text.RegularExpressions;

namespace Accredia.GestioneAnagrafica.API.Validators
{
    /// <summary>
    /// Validatore per Codice Fiscale italiano ed estero
    /// </summary>
    public static class CodiceFiscaleValidator
    {
        /// <summary>
        /// Valida il codice fiscale
        /// Accetta:
        /// - Codici fiscali italiani (16 caratteri alfanumerici)
        /// - "N/D" per non disponibile
        /// - "ESTERO" per stranieri
        /// - Codici esteri personalizzati
        /// </summary>
        public static ValidationResult Validate(string codiceFiscale)
        {
            if (string.IsNullOrWhiteSpace(codiceFiscale))
                return ValidationResult.Invalid("Codice fiscale non può essere vuoto");

            var codice = codiceFiscale.ToUpper().Trim();

            // Valori speciali accettati
            if (codice == "N/D" || codice == "ESTERO" || codice == "SCONOSCIUTO")
                return ValidationResult.Valid();

            // Codice fiscale italiano (16 caratteri)
            if (codice.Length == 16 && IsValidItalianCodiceFiscale(codice))
                return ValidationResult.Valid();

            // Codice fiscale estero (se ha formato internazionale)
            if (IsValidInternationalCode(codice))
                return ValidationResult.Valid();

            return ValidationResult.Invalid($"Codice fiscale '{codiceFiscale}' non valido. Accettati: CF italiano (16 char), 'N/D', 'ESTERO', o codici internazionali");
        }

        /// <summary>
        /// Valida un codice fiscale italiano secondo l'algoritmo ufficiale
        /// </summary>
        private static bool IsValidItalianCodiceFiscale(string codiceFiscale)
        {
            if (string.IsNullOrWhiteSpace(codiceFiscale) || codiceFiscale.Length != 16)
                return false;

            // Formato: RSSMRA12A345K123
            // 6 char cognome, 3 char nome, 2 char anno, 1 char mese, 2 char giorno, 1 char comune, 3 char numero, 1 char controllo

            // Verifica formato generale
            if (!Regex.IsMatch(codiceFiscale, @"^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$"))
                return false;

            // Calcolo del carattere di controllo
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int[] num = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };

            int somma = 0;

            for (int i = 0; i < 15; i++)
            {
                int val;
                if (char.IsLetter(codiceFiscale[i]))
                {
                    val = num[chars.IndexOf(codiceFiscale[i])];
                }
                else
                {
                    if (i % 2 == 0)
                        val = int.Parse(codiceFiscale[i].ToString());
                    else
                        val = int.Parse(codiceFiscale[i].ToString()) * 2;
                    if (val > 9)
                        val -= 9;
                }

                somma += val;
            }

            int resto = somma % 26;
            char carattereControllo = chars[resto];

            return codiceFiscale[15] == carattereControllo;
        }

        /// <summary>
        /// Valida codici fiscali internazionali
        /// Formati accettati:
        /// - Codici alfanumerici di lunghezza 5-30 caratteri
        /// - Non deve contenere caratteri speciali (solo lettere, numeri, trattino, underscore)
        /// </summary>
        private static bool IsValidInternationalCode(string codiceFiscale)
        {
            if (string.IsNullOrWhiteSpace(codiceFiscale))
                return false;

            if (codiceFiscale.Length < 5 || codiceFiscale.Length > 30)
                return false;

            // Formato internazionale: solo alfanumerici, trattini, underscore
            return Regex.IsMatch(codiceFiscale, @"^[A-Z0-9\-_]+$");
        }
    }

    /// <summary>
    /// Risultato di una validazione
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ValidationResult Valid() => new() { IsValid = true };
        public static ValidationResult Invalid(string message) => new() { IsValid = false, Message = message };
    }

    /// <summary>
    /// Attributo di validazione personalizzato per DataAnnotations
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidCodiceFiscaleAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        protected override System.ComponentModel.DataAnnotations.ValidationResult? IsValid(object? value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new System.ComponentModel.DataAnnotations.ValidationResult("Codice fiscale è obbligatorio");

            var result = CodiceFiscaleValidator.Validate(value.ToString()!);

            return result.IsValid 
                ? System.ComponentModel.DataAnnotations.ValidationResult.Success 
                : new System.ComponentModel.DataAnnotations.ValidationResult(result.Message);
        }
    }
}
