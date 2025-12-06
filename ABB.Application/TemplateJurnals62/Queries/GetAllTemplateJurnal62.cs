using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TemplateJurnals62.Queries
{
    public class GetAllTemplateJurnal62Query : IRequest<List<TemplateJurnal62Dto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetAllTemplateJurnal62QueryHandler 
        : IRequestHandler<GetAllTemplateJurnal62Query, List<TemplateJurnal62Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTemplateJurnal62QueryHandler> _logger;

        public GetAllTemplateJurnal62QueryHandler(
            IDbContextPstNota context,
            IMapper mapper,
            ILogger<GetAllTemplateJurnal62QueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TemplateJurnal62Dto>> Handle(
            GetAllTemplateJurnal62Query request,
            CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.TemplateJurnal62.AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchKeyword))
                {
                    var keyword = request.SearchKeyword.Trim();

                    query = query.Where(x =>
                        x.Type.Contains(keyword) ||
                        x.NamaJurnal.Contains(keyword) ||
                        x.JenisAss.Contains(keyword)
                    );
                }

                return await query
                    .ProjectTo<TemplateJurnal62Dto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saat mengambil Template Jurnal 62");
                throw;
            }
        }
    }
}
