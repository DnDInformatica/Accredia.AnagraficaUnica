using FluentValidation;
using GestioneOrganismi.Backend.DTOs;

namespace GestioneOrganismi.Backend.Validators
{
    public class DocumentoUploadValidator : AbstractValidator<DocumentoDTO.Upload>
    {
        public DocumentoUploadValidator()
        {
            RuleFor(x => x.NomeFile)
                .NotEmpty().WithMessage("Il nome del file è obbligatorio")
                .MaximumLength(510).WithMessage("Il nome del file non può superare 510 caratteri");

            RuleFor(x => x.MimeType)
                .NotEmpty().WithMessage("Il tipo MIME è obbligatorio")
                .MaximumLength(200).WithMessage("Il tipo MIME non può superare 200 caratteri")
                .Must(mime => new[] { 
                    "application/pdf", 
                    "image/jpeg", 
                    "image/png", 
                    "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/vnd.ms-excel",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                }.Contains(mime))
                    .WithMessage("Tipo di file non supportato. Formati ammessi: PDF, JPEG, PNG, DOC, DOCX, XLS, XLSX");

            RuleFor(x => x.ContenutoBase64)
                .NotEmpty().WithMessage("Il contenuto del file è obbligatorio")
                .When(x => !string.IsNullOrWhiteSpace(x.ContenutoBase64));

            RuleFor(x => x.Note)
                .MaximumLength(2000).WithMessage("Le note non possono superare 2000 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));

            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'entità aziendale deve essere valida")
                .When(x => x.EntitaAziendaleId.HasValue);
        }
    }
}
