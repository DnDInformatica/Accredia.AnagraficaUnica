using FluentValidation;
using Accredia.GestioneAnagrafica.Shared.DTOs;

namespace Accredia.GestioneAnagrafica.API.Validators
{
    public class EnteAccreditamentoCreateValidator : AbstractValidator<EnteAccreditamentoDTO.Create>
    {
        public EnteAccreditamentoCreateValidator()
        {
            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'ID dell'Entità Aziendale è obbligatorio");

            RuleFor(x => x.Denominazione)
                .NotEmpty().WithMessage("La denominazione dell'Ente di Accreditamento è obbligatoria")
                .MaximumLength(250).WithMessage("La denominazione non può superare 250 caratteri");

            RuleFor(x => x.Sigla)
                .MaximumLength(50).WithMessage("La sigla non può superare 50 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Sigla));

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Le note non possono superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));

            RuleFor(x => x.DataFondazione)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di fondazione non può essere futura")
                .When(x => x.DataFondazione.HasValue);
        }
    }

    public class EnteAccreditamentoUpdateValidator : AbstractValidator<EnteAccreditamentoDTO.Update>
    {
        public EnteAccreditamentoUpdateValidator()
        {
            RuleFor(x => x.Denominazione)
                .NotEmpty().WithMessage("La denominazione dell'Ente di Accreditamento è obbligatoria")
                .MaximumLength(250).WithMessage("La denominazione non può superare 250 caratteri");

            RuleFor(x => x.Sigla)
                .MaximumLength(50).WithMessage("La sigla non può superare 50 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Sigla));

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Le note non possono superare 500 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));

            RuleFor(x => x.DataFondazione)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("La data di fondazione non può essere futura")
                .When(x => x.DataFondazione.HasValue);
        }
    }
}
