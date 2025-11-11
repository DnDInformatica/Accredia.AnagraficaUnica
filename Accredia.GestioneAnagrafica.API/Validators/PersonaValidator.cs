using FluentValidation;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;

namespace Accredia.GestioneAnagrafica.API.Validators;

public class CreatePersonaValidator : AbstractValidator<CreatePersonaRequest>
{
    public CreatePersonaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Il nome è obbligatorio")
            .MaximumLength(100).WithMessage("Il nome non può superare 100 caratteri")
            .Matches(@"^[a-zA-ZàèéìòùÀÈÉÌÒÙ\s'-]+$")
                .WithMessage("Il nome può contenere solo lettere, spazi, apostrofi e trattini");

        RuleFor(x => x.Cognome)
            .NotEmpty().WithMessage("Il cognome è obbligatorio")
            .MaximumLength(100).WithMessage("Il cognome non può superare 100 caratteri")
            .Matches(@"^[a-zA-ZàèéìòùÀÈÉÌÒÙ\s'-]+$")
                .WithMessage("Il cognome può contenere solo lettere, spazi, apostrofi e trattini");

        RuleFor(x => x.CodiceFiscale)
            .NotEmpty().WithMessage("Il codice fiscale è obbligatorio")
            .MaximumLength(20).WithMessage("Il codice fiscale non può superare 20 caratteri")
            .Must(BeValidCodiceFiscale)
                .WithMessage("Codice fiscale non valido. Formato accettato: 16 caratteri alfanumerici, oppure 'ESTERO', 'N/D' per soggetti esteri");

        RuleFor(x => x.Genere)
            .NotEmpty().WithMessage("Il genere è obbligatorio")
            .Must(g => g == "M" || g == "F" || g == "O")
                .WithMessage("Il genere deve essere M (Maschio), F (Femmina) o O (Altro)");

        RuleFor(x => x.EntitaAziendaleId)
            .GreaterThan(0).WithMessage("L'EntitàAziendaleId deve essere maggiore di 0");

        RuleFor(x => x.DataNascita)
            .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("La data di nascita non può essere futura")
            .GreaterThan(new DateTime(1900, 1, 1))
                .WithMessage("La data di nascita non può essere precedente al 1900")
            .When(x => x.DataNascita.HasValue);

        RuleFor(x => x.LuogoNascita)
            .MaximumLength(100).WithMessage("Il luogo di nascita non può superare 100 caratteri")
            .When(x => !string.IsNullOrWhiteSpace(x.LuogoNascita));

        RuleFor(x => x.Qualifica)
            .MaximumLength(100).WithMessage("La qualifica non può superare 100 caratteri")
            .When(x => !string.IsNullOrWhiteSpace(x.Qualifica));

        RuleFor(x => x.Titolo)
            .MaximumLength(50).WithMessage("Il titolo non può superare 50 caratteri")
            .When(x => !string.IsNullOrWhiteSpace(x.Titolo));

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Le note non possono superare 500 caratteri")
            .When(x => !string.IsNullOrWhiteSpace(x.Note));

        RuleFor(x => x.DataConsensoPrivacy)
            .NotNull()
                .WithMessage("La data del consenso privacy è obbligatoria quando il consenso è attivo")
            .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("La data del consenso privacy non può essere futura")
            .When(x => x.PrivacyConsent);
    }

    private bool BeValidCodiceFiscale(string codiceFiscale)
    {
        if (string.IsNullOrWhiteSpace(codiceFiscale))
            return false;

        var upperCF = codiceFiscale.ToUpperInvariant();
        if (upperCF == "ESTERO" || upperCF == "N/D" || upperCF == "ND")
            return true;

        if (codiceFiscale.Length != 16)
            return false;

        var pattern = @"^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$";
        return System.Text.RegularExpressions.Regex.IsMatch(codiceFiscale.ToUpperInvariant(), pattern);
    }
}

public class UpdatePersonaValidator : AbstractValidator<UpdatePersonaRequest>
{
    public UpdatePersonaValidator()
    {
        Include(new CreatePersonaValidator());

        RuleFor(x => x.EntitaAziendaleId)
            .GreaterThan(0).WithMessage("Il EntitaAziendaleId deve essere maggiore di 0");
    }
}
