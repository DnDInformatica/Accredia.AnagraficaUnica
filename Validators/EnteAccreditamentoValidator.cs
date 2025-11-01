using FluentValidation;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Models;
using System;

namespace GestioneOrganismi.Backend.Validators
{
    public class EnteAccreditamentoCreateValidator : AbstractValidator<EnteAccreditamentoDTO.Create>
    {
        public EnteAccreditamentoCreateValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Il nome dell'Ente di Accreditamento è obbligatorio")
                .MaximumLength(250).WithMessage("Il nome non può superare 250 caratteri");

            RuleFor(x => x.Codice)
                .NotEmpty().WithMessage("Il codice identificativo è obbligatorio")
                .MaximumLength(50).WithMessage("Il codice identificativo non può superare 50 caratteri")
                .Matches(@"^[A-Za-z0-9\-_]+$").WithMessage("Il codice identificativo contiene caratteri non validi");

            RuleFor(x => x.Descrizione)
                .MaximumLength(500).WithMessage("La descrizione non può superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Descrizione));

            RuleFor(x => x.SettoreMerceologico)
                .MaximumLength(100).WithMessage("Il settore merceologico non può superare 100 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.SettoreMerceologico));

            RuleFor(x => x.DataAccreditamento)
                .NotEmpty().WithMessage("La data di accreditamento è obbligatoria")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di accreditamento non può essere futura");
        }
    }

    public class EnteAccreditamentoUpdateValidator : AbstractValidator<EnteAccreditamentoDTO.Update>
    {
        public EnteAccreditamentoUpdateValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Il nome dell'Ente di Accreditamento è obbligatorio")
                .MaximumLength(250).WithMessage("Il nome non può superare 250 caratteri");

            RuleFor(x => x.Codice)
                .NotEmpty().WithMessage("Il codice identificativo è obbligatorio")
                .MaximumLength(50).WithMessage("Il codice identificativo non può superare 50 caratteri")
                .Matches(@"^[A-Za-z0-9\-_]+$").WithMessage("Il codice identificativo contiene caratteri non validi");

            RuleFor(x => x.Descrizione)
                .MaximumLength(500).WithMessage("La descrizione non può superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Descrizione));

            RuleFor(x => x.SettoreMerceologico)
                .MaximumLength(100).WithMessage("Il settore merceologico non può superare 100 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.SettoreMerceologico));

            RuleFor(x => x.DataAccreditamento)
                .NotEmpty().WithMessage("La data di accreditamento è obbligatoria")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di accreditamento non può essere futura");

            RuleFor(x => x.Stato)
                .Must(x => Enum.IsDefined(typeof(EnteAccreditamento.StatoAccreditamento), x))
                .WithMessage("Stato di accreditamento non valido");
        }
    }
}
