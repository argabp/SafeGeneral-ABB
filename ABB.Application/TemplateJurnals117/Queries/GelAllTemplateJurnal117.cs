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

namespace ABB.Application.TemplateJurnals117.Queries
{
    public class GetAllTemplateJurnal117Query : IRequest<List<TemplateJurnal117Dto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetAllTemplateJurnal117QueryHandler 
        : IRequestHandler<GetAllTemplateJurnal117Query, List<TemplateJurnal117Dto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTemplateJurnal117QueryHandler> _logger;

        public GetAllTemplateJurnal117QueryHandler(
            IDbContextPstNota context,
            IMapper mapper,
            ILogger<GetAllTemplateJurnal117QueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TemplateJurnal117Dto>> Handle(
            GetAllTemplateJurnal117Query request,
            CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.TemplateJurnal117.AsQueryable();

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
                    .ProjectTo<TemplateJurnal117Dto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saat mengambil Template Jurnal 117");
                throw;
            }
        }
    }
}
