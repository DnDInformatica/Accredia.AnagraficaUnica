using FluentValidation;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using System.Text.RegularExpressions;

namespace Accredia.GestioneAnagrafica.API.Validators
{
    /// <summary>
    /// Validator per la creazione di Telefoni
    /// </summary>
    public class TelefonoCreateValidator : AbstractValidator<TelefonoDTO.Create>
    {
        public TelefonoCreateValidator()
        {
            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'EntitaAziendaleId deve essere maggiore di zero");

            RuleFor(x => x.TipoTelefonoId)
                .GreaterThan(0).WithMessage("Il TipoTelefonoId deve essere maggiore di zero");

            RuleFor(x => x.PrefissoInternazionale)
                .MaximumLength(20).WithMessage("Il prefisso internazionale non può superare 20 caratteri")
                .Must(BeValidInternationalPrefix).WithMessage("Il prefisso internazionale deve iniziare con + seguito da numeri (es. +39, +1)")
                .When(x => !string.IsNullOrWhiteSpace(x.PrefissoInternazionale));

            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("Il numero di telefono è obbligatorio")
                .MaximumLength(100).WithMessage("Il numero di telefono non può superare 100 caratteri")
                .Must(BeValidPhoneNumber).WithMessage("Il numero di telefono contiene caratteri non validi. Sono ammessi solo numeri, spazi, trattini e parentesi");

            RuleFor(x => x.Estensione)
                .MaximumLength(20).WithMessage("L'estensione non può superare 20 caratteri")
                .Must(BeValidExtension).WithMessage("L'estensione deve contenere solo numeri")
                .When(x => !string.IsNullOrWhiteSpace(x.Estensione));

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Le note non possono superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));
        }

        /// <summary>
        /// Validazione per prefisso internazionale (es. +39, +1, +44)
        /// </summary>
        private bool BeValidInternationalPrefix(string? prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return true;

            // Deve iniziare con + seguito da 1-4 cifre
            return Regex.IsMatch(prefix, @"^\+[0-9]{1,4}$");
        }

        /// <summary>
        /// Validazione per numero di telefono
        /// Accetta: numeri, spazi, trattini, parentesi
        /// </summary>
        private bool BeValidPhoneNumber(string? numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return false;

            // Pattern per numero di telefono: numeri, spazi, trattini, parentesi
            var phonePattern = @"^[0-9\s\-\(\)\/]+$";
            return Regex.IsMatch(numero, phonePattern);
        }

        /// <summary>
        /// Validazione per estensione telefonica
        /// </summary>
        private bool BeValidExtension(string? estensione)
        {
            if (string.IsNullOrWhiteSpace(estensione))
                return true;

            // Solo numeri per l'estensione
            return Regex.IsMatch(estensione, @"^[0-9]+$");
        }
    }

    /// <summary>
    /// Validator per l'aggiornamento di Telefoni
    /// </summary>
    public class TelefonoUpdateValidator : AbstractValidator<TelefonoDTO.Update>
    {
        public TelefonoUpdateValidator()
        {
            RuleFor(x => x.TipoTelefonoId)
                .GreaterThan(0).WithMessage("Il TipoTelefonoId deve essere maggiore di zero");

            RuleFor(x => x.PrefissoInternazionale)
                .MaximumLength(20).WithMessage("Il prefisso internazionale non può superare 20 caratteri")
                .Must(BeValidInternationalPrefix).WithMessage("Il prefisso internazionale deve iniziare con + seguito da numeri (es. +39, +1)")
                .When(x => !string.IsNullOrWhiteSpace(x.PrefissoInternazionale));

            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("Il numero di telefono è obbligatorio")
                .MaximumLength(100).WithMessage("Il numero di telefono non può superare 100 caratteri")
                .Must(BeValidPhoneNumber).WithMessage("Il numero di telefono contiene caratteri non validi. Sono ammessi solo numeri, spazi, trattini e parentesi");

            RuleFor(x => x.Estensione)
                .MaximumLength(20).WithMessage("L'estensione non può superare 20 caratteri")
                .Must(BeValidExtension).WithMessage("L'estensione deve contenere solo numeri")
                .When(x => !string.IsNullOrWhiteSpace(x.Estensione));

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Le note non possono superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));
        }

        /// <summary>
        /// Validazione per prefisso internazionale (es. +39, +1, +44)
        /// </summary>
        private bool BeValidInternationalPrefix(string? prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return true;

            // Deve iniziare con + seguito da 1-4 cifre
            return Regex.IsMatch(prefix, @"^\+[0-9]{1,4}$");
        }

        /// <summary>
        /// Validazione per numero di telefono
        /// Accetta: numeri, spazi, trattini, parentesi
        /// </summary>
        private bool BeValidPhoneNumber(string? numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                return false;

            // Pattern per numero di telefono: numeri, spazi, trattini, parentesi
            var phonePattern = @"^[0-9\s\-\(\)\/]+$";
            return Regex.IsMatch(numero, phonePattern);
        }

        /// <summary>
        /// Validazione per estensione telefonica
        /// </summary>
        private bool BeValidExtension(string? estensione)
        {
            if (string.IsNullOrWhiteSpace(estensione))
                return true;

            // Solo numeri per l'estensione
            return Regex.IsMatch(estensione, @"^[0-9]+$");
        }
    }
}
