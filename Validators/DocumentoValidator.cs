using FluentValidation;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Services;
using Microsoft.Extensions.Options;
using GestioneOrganismi.Backend.Config;

namespace GestioneOrganismi.Backend.Validators
{
    public class DocumentoUploadValidator : AbstractValidator<DocumentoDTO.Upload>
    {
        private readonly DocumentStorageConfig _config;

        public DocumentoUploadValidator(IOptions<DocumentStorageConfig> config)
        {
            _config = config.Value;

            RuleFor(x => x.NomeFile)
                .NotEmpty().WithMessage("Il nome del file è obbligatorio")
                .MaximumLength(510).WithMessage("Il nome del file non può superare 510 caratteri");

            RuleFor(x => x.MimeType)
                .NotEmpty().WithMessage("Il tipo MIME è obbligatorio")
                .MaximumLength(200).WithMessage("Il tipo MIME non può superare 200 caratteri")
                .Must(mime => _config.AllowedMimeTypes.Contains(mime.ToLowerInvariant()))
                    .WithMessage(x => $"Tipo di file non supportato: {x.MimeType}. Controllare la configurazione AllowedMimeTypes.");

            RuleFor(x => x.NomeFile)
                .Must(fileName => {
                    var extension = Path.GetExtension(fileName).ToLowerInvariant();
                    return _config.AllowedExtensions.Contains(extension);
                })
                .WithMessage(x => $"Estensione file non supportata: {Path.GetExtension(x.NomeFile)}. Controllare la configurazione AllowedExtensions.");

            RuleFor(x => x.ContenutoBase64)
                .NotEmpty().WithMessage("Il contenuto del file è obbligatorio")
                .When(x => !string.IsNullOrWhiteSpace(x.ContenutoBase64))
                .Must(base64 => {
                    if (string.IsNullOrWhiteSpace(base64))
                        return true;
                    
                    try
                    {
                        var bytes = Convert.FromBase64String(base64);
                        var sizeMB = bytes.Length / (1024.0 * 1024.0);
                        return sizeMB <= _config.MaxFileSizeMB;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage(x => $"Il file supera la dimensione massima consentita di {_config.MaxFileSizeMB} MB");

            RuleFor(x => x.Note)
                .MaximumLength(2000).WithMessage("Le note non possono superare 2000 caratteri")
                .When(x => !string.IsNullOrWhiteSpace(x.Note));

            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'entità aziendale deve essere valida")
                .When(x => x.EntitaAziendaleId.HasValue);
        }
    }
}
