using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Models;
using GestioneOrganismi.Backend.Responses;
using AutoMapper;
using System.Security.Claims;

namespace GestioneOrganismi.Backend.Endpoints.EntiAccreditamento
{
    public class CreateEnteAccreditamentoEndpoint
    {
        private readonly PersoneDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _user;

        public CreateEnteAccreditamentoEndpoint(
            PersoneDbContext context, 
            IMapper mapper,
            ClaimsPrincipal user)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
        }

        public async Task<ApiResponse<GestioneOrganismi.Backend.DTOs.EnteAccreditamentoDTO.Response>> CreateAsync(GestioneOrganismi.Backend.DTOs.EnteAccreditamentoDTO.Create dto)
        {
            // Validazione codice identificativo univoco
            var esisteGià = await _context.EntiAccreditamento
                .AnyAsync(e => e.CodiceIdentificativo == dto.Codice && !e.IsDeleted);

            if (esisteGià)
            {
                return ApiResponse<GestioneOrganismi.Backend.DTOs.EnteAccreditamentoDTO.Response>.ErrorResponse(
                    "Un Ente di Accreditamento con questo codice identificativo esiste già.");
            }

            var enteAccreditamento = _mapper.Map<EnteAccreditamento>(dto);

            // Impostazione campi di auditing
            enteAccreditamento.CreatedBy = _user.FindFirstValue(ClaimTypes.Name) ?? "Sistema";
            enteAccreditamento.CreatedAt = DateTime.UtcNow;
            enteAccreditamento.Stato = EnteAccreditamento.StatoAccreditamento.InIstruttoria;

            _context.EntiAccreditamento.Add(enteAccreditamento);
            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<GestioneOrganismi.Backend.DTOs.EnteAccreditamentoDTO.Response>(enteAccreditamento);

            return ApiResponse<GestioneOrganismi.Backend.DTOs.EnteAccreditamentoDTO.Response>.SuccessResponse(
                responseDto, 
                "Ente di Accreditamento creato con successo.");
        }
    }
}