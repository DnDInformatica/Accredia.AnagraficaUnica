using FluentValidation;
using Accredia.GestioneAnagrafica.API.DTOs;

namespace Accredia.GestioneAnagrafica.API.Validators
{
    /// <summary>
    /// Validator per la creazione di un Organismo Accreditato
    /// </summary>
    public class OrganismoAccreditatoCreateValidator : AbstractValidator<OrganismoAccreditatoDTO.Create>
    {
        public OrganismoAccreditatoCreateValidator()
        {
            RuleFor(x => x.RagioneSociale)
                .NotEmpty().WithMessage("La ragione sociale è obbligatoria")
                .MaximumLength(510).WithMessage("La ragione sociale non può superare 510 caratteri");

            RuleFor(x => x.PartitaIVA)
                .MaximumLength(100).WithMessage("La Partita IVA non può superare 100 caratteri")
                .Matches(@"^\d{11}$").WithMessage("La Partita IVA deve contenere esattamente 11 cifre")
                .When(x => !string.IsNullOrWhiteSpace(x.PartitaIVA));

            RuleFor(x => x.CodiceFiscale)
                .MaximumLength(100).WithMessage("Il Codice Fiscale non può superare 100 caratteri")
                .Matches(@"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$")
                    .WithMessage("Il Codice Fiscale non è in formato valido")
                .When(x => !string.IsNullOrWhiteSpace(x.CodiceFiscale));

            RuleFor(x => x.EnteAccreditamentoId)
                .GreaterThan(0).WithMessage("L'Ente Accreditamento deve essere valido")
                .When(x => x.EnteAccreditamentoId.HasValue);
        }
    }

    /// <summary>
    /// Validator per l'aggiornamento di un Organismo Accreditato
    /// </summary>
    public class OrganismoAccreditatoUpdateValidator : AbstractValidator<OrganismoAccreditatoDTO.Update>
    {
        public OrganismoAccreditatoUpdateValidator()
        {
            RuleFor(x => x.RagioneSociale)
                .NotEmpty().WithMessage("La ragione sociale è obbligatoria")
                .MaximumLength(510).WithMessage("La ragione sociale non può superare 510 caratteri");

            RuleFor(x => x.PartitaIVA)
                .MaximumLength(100).WithMessage("La Partita IVA non può superare 100 caratteri")
                .Matches(@"^\d{11}$").WithMessage("La Partita IVA deve contenere esattamente 11 cifre")
                .When(x => !string.IsNullOrWhiteSpace(x.PartitaIVA));

            RuleFor(x => x.CodiceFiscale)
                .MaximumLength(100).WithMessage("Il Codice Fiscale non può superare 100 caratteri")
                .Matches(@"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$")
                    .WithMessage("Il Codice Fiscale non è in formato valido")
                .When(x => !string.IsNullOrWhiteSpace(x.CodiceFiscale));

            RuleFor(x => x.EnteAccreditamentoId)
                .GreaterThan(0).WithMessage("L'Ente Accreditamento deve essere valido")
                .When(x => x.EnteAccreditamentoId.HasValue);
        }
    }
}
