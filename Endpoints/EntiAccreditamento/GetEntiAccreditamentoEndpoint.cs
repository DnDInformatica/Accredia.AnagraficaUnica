using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Responses;
using AutoMapper;

namespace GestioneOrganismi.Backend.Endpoints.EntiAccreditamento
{
    public class GetEntiAccreditamentoEndpoint
    {
        private readonly PersoneDbContext _context;
        private readonly IMapper _mapper;

        public GetEntiAccreditamentoEndpoint(PersoneDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PageResult<EnteAccreditamentoDTO.Response>>> GetAsync(
            int page = 1, 
            int pageSize = 10, 
            string? searchTerm = null, 
            string? sortBy = null, 
            bool ascending = true)
        {
            var query = _context.EntiAccreditamento
                .Where(e => !e.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => 
                    e.Nome.Contains(searchTerm) || 
                    e.CodiceIdentificativo.Contains(searchTerm) || 
                    e.SettoreMerceologico.Contains(searchTerm));
            }

            // Implementazione ordinamento dinamico
            query = sortBy?.ToLower() switch
            {
                "nome" => ascending ? query.OrderBy(e => e.Nome) : query.OrderByDescending(e => e.Nome),
                "codiceidentificativo" => ascending ? query.OrderBy(e => e.CodiceIdentificativo) : query.OrderByDescending(e => e.CodiceIdentificativo),
                "dataaccreditamento" => ascending ? query.OrderBy(e => e.DataAccreditamento) : query.OrderByDescending(e => e.DataAccreditamento),
                _ => query.OrderBy(e => e.Id)
            };

            var totalCount = await query.CountAsync();

            var entiAccreditamento = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var entiDto = _mapper.Map<List<EnteAccreditamentoDTO.Response>>(entiAccreditamento);

            return ApiResponse<PageResult<EnteAccreditamentoDTO.Response>>.SuccessResponse(new PageResult<EnteAccreditamentoDTO.Response>
            {
                Data = entiDto,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            });
        }
    }
}