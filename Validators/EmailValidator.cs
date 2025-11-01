using FluentValidation;
using GestioneOrganismi.Backend.DTOs;
using System.Text.RegularExpressions;

namespace GestioneOrganismi.Backend.Validators
{
    /// <summary>
    /// Validator per la creazione di Email
    /// </summary>
    public class EmailCreateValidator : AbstractValidator<EmailDTO.Create>
    {
        public EmailCreateValidator()
        {
            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'EntitaAziendaleId deve essere maggiore di zero");

            RuleFor(x => x.TipoEmailId)
                .GreaterThan(0).WithMessage("Il TipoEmailId deve essere maggiore di zero");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'indirizzo email è obbligatorio")
                .MaximumLength(510).WithMessage("L'indirizzo email non può superare 510 caratteri")
                .EmailAddress().WithMessage("L'indirizzo email non è valido")
                .Must(BeValidEmailFormat).WithMessage("L'indirizzo email contiene caratteri non validi o formattazione non corretta");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Le note non possono superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));
        }

        /// <summary>
        /// Validazione aggiuntiva per formato email più rigorosa
        /// </summary>
        private bool BeValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Pattern RFC 5322 semplificato per email
            var emailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            
            return Regex.IsMatch(email, emailPattern);
        }
    }

    /// <summary>
    /// Validator per l'aggiornamento di Email
    /// </summary>
    public class EmailUpdateValidator : AbstractValidator<EmailDTO.Update>
    {
        public EmailUpdateValidator()
        {
            RuleFor(x => x.TipoEmailId)
                .GreaterThan(0).WithMessage("Il TipoEmailId deve essere maggiore di zero");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'indirizzo email è obbligatorio")
                .MaximumLength(510).WithMessage("L'indirizzo email non può superare 510 caratteri")
                .EmailAddress().WithMessage("L'indirizzo email non è valido")
                .Must(BeValidEmailFormat).WithMessage("L'indirizzo email contiene caratteri non validi o formattazione non corretta");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Le note non possono superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));
        }

        /// <summary>
        /// Validazione aggiuntiva per formato email più rigorosa
        /// </summary>
        private bool BeValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Pattern RFC 5322 semplificato per email
            var emailPattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
