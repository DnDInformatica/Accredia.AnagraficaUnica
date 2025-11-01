using FluentValidation;
using GestioneOrganismi.Backend.DTOs;

namespace GestioneOrganismi.Backend.Validators
{
    public class RilascioAccreditamentoCreateValidator : AbstractValidator<RilascioAccreditamentoDTO.Create>
    {
        public RilascioAccreditamentoCreateValidator()
        {
            RuleFor(x => x.EnteAccreditamentoId)
                .GreaterThan(0).WithMessage("L'Ente Accreditamento è obbligatorio");

            RuleFor(x => x.EnteAccreditatoId)
                .GreaterThan(0).WithMessage("L'Ente Accreditato è obbligatorio");

            RuleFor(x => x.NumeroAtto)
                .MaximumLength(200).WithMessage("Il numero atto non può superare 200 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.NumeroAtto));

            RuleFor(x => x.DataRilascio)
                .NotEmpty().WithMessage("La data di rilascio è obbligatoria")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di rilascio non può essere futura");

            RuleFor(x => x.DataScadenza)
                .GreaterThan(x => x.DataRilascio)
                    .WithMessage("La data di scadenza deve essere successiva alla data di rilascio")
                .When(x => x.DataScadenza.HasValue);

            RuleFor(x => x.Stato)
                .NotEmpty().WithMessage("Lo stato è obbligatorio")
                .MaximumLength(100).WithMessage("Lo stato non può superare 100 caratteri")
                .Must(stato => new[] { "InLavorazione", "Attivo", "Sospeso", "Revocato", "Scaduto" }.Contains(stato))
                    .WithMessage("Lo stato deve essere uno dei seguenti: InLavorazione, Attivo, Sospeso, Revocato, Scaduto");
        }
    }

    public class RilascioAccreditamentoUpdateValidator : AbstractValidator<RilascioAccreditamentoDTO.Update>
    {
        public RilascioAccreditamentoUpdateValidator()
        {
            RuleFor(x => x.NumeroAtto)
                .MaximumLength(200).WithMessage("Il numero atto non può superare 200 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.NumeroAtto));

            RuleFor(x => x.DataRilascio)
                .NotEmpty().WithMessage("La data di rilascio è obbligatoria")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di rilascio non può essere futura");

            RuleFor(x => x.DataScadenza)
                .GreaterThan(x => x.DataRilascio)
                    .WithMessage("La data di scadenza deve essere successiva alla data di rilascio")
                .When(x => x.DataScadenza.HasValue);

            RuleFor(x => x.Stato)
                .NotEmpty().WithMessage("Lo stato è obbligatorio")
                .MaximumLength(100).WithMessage("Lo stato non può superare 100 caratteri")
                .Must(stato => new[] { "InLavorazione", "Attivo", "Sospeso", "Revocato", "Scaduto" }.Contains(stato))
                    .WithMessage("Lo stato deve essere uno dei seguenti: InLavorazione, Attivo, Sospeso, Revocato, Scaduto");
        }
    }
}
