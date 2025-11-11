using AutoMapper;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;

namespace Accredia.GestioneAnagrafica.API.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // EnteAccreditamento mappings
            CreateMap<EnteAccreditamento, EnteAccreditamentoDTO.Response>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EntitaAziendaleId));

            CreateMap<EnteAccreditamentoDTO.Create, EnteAccreditamento>()
                .ForMember(dest => dest.EntitaAziendaleId, opt => opt.MapFrom(src => src.EntitaAziendaleId))
                .ForMember(dest => dest.Denominazione, opt => opt.MapFrom(src => src.Denominazione))
                .ForMember(dest => dest.Sigla, opt => opt.MapFrom(src => src.Sigla))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.DataFondazione, opt => opt.MapFrom(src => src.DataFondazione));

            CreateMap<EnteAccreditamentoDTO.Update, EnteAccreditamento>()
                .ForMember(dest => dest.Denominazione, opt => opt.MapFrom(src => src.Denominazione))
                .ForMember(dest => dest.Sigla, opt => opt.MapFrom(src => src.Sigla))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.DataFondazione, opt => opt.MapFrom(src => src.DataFondazione));

            // RilascioAccreditamento mappings
            CreateMap<RilascioAccreditamento, RilascioAccreditamentoDTO.Response>();
            CreateMap<RilascioAccreditamentoDTO.Create, RilascioAccreditamento>();
            CreateMap<RilascioAccreditamentoDTO.Update, RilascioAccreditamento>();

            // OrganismoAccreditato mappings
            CreateMap<OrganismoAccreditato, OrganismoAccreditatoDTO.Response>();
            CreateMap<OrganismoAccreditato, OrganismoAccreditatoDTO.List>();
            CreateMap<OrganismoAccreditatoDTO.Create, OrganismoAccreditato>();
            CreateMap<OrganismoAccreditatoDTO.Update, OrganismoAccreditato>();
        }
    }
}
