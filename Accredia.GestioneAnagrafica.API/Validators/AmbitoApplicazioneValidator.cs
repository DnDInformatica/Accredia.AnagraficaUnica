using FluentValidation;
using Accredia.GestioneAnagrafica.API.DTOs;

namespace Accredia.GestioneAnagrafica.API.Validators
{
    public class AmbitoApplicazioneCreateValidator : AbstractValidator<AmbitoApplicazioneDTO.Create>
    {
        public AmbitoApplicazioneCreateValidator()
        {
            RuleFor(x => x.Codice)
                .NotEmpty().WithMessage("Il codice è obbligatorio")
                .MaximumLength(100).WithMessage("Il codice non può superare 100 caratteri")
                .Matches(@"^[A-Za-z0-9\-_]+$")
                    .WithMessage("Il codice può contenere solo lettere, numeri, trattini e underscore");

            RuleFor(x => x.Denominazione)
                .NotEmpty().WithMessage("La denominazione è obbligatoria")
                .MaximumLength(200).WithMessage("La denominazione non può superare 200 caratteri");

            RuleFor(x => x.Descrizione)
                .MaximumLength(1000).WithMessage("La descrizione non può superare 1000 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Descrizione));

            RuleFor(x => x.Ordine)
                .GreaterThanOrEqualTo(0).WithMessage("L'ordine deve essere maggiore o uguale a 0");
        }
    }

    public class AmbitoApplicazioneUpdateValidator : AbstractValidator<AmbitoApplicazioneDTO.Update>
    {
        public AmbitoApplicazioneUpdateValidator()
        {
            RuleFor(x => x.Codice)
                .NotEmpty().WithMessage("Il codice è obbligatorio")
                .MaximumLength(100).WithMessage("Il codice non può superare 100 caratteri")
                .Matches(@"^[A-Za-z0-9\-_]+$")
                    .WithMessage("Il codice può contenere solo lettere, numeri, trattini e underscore");

            RuleFor(x => x.Denominazione)
                .NotEmpty().WithMessage("La denominazione è obbligatoria")
                .MaximumLength(200).WithMessage("La denominazione non può superare 200 caratteri");

            RuleFor(x => x.Descrizione)
                .MaximumLength(1000).WithMessage("La descrizione non può superare 1000 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Descrizione));

            RuleFor(x => x.Ordine)
                .GreaterThanOrEqualTo(0).WithMessage("L'ordine deve essere maggiore o uguale a 0");
        }
    }
}
