using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class RecalculatePremiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class RecalculatePremiQueryHandler : IRequestHandler<RecalculatePremiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IMapper _mapper;

        public RecalculatePremiQueryHandler(IDbConnectionFactory connectionFactory, IMapper mapper)
        {
            _connectionFactory = connectionFactory;
            _mapper = mapper;
        }

        public async Task<string> Handle(RecalculatePremiQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var results = (await _connectionFactory.QueryProc<(string, string, string)>("spp_uw02e_02", new
            {
                request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                request.no_aks, request.no_updt
            })).FirstOrDefault();

            return results.Item2;
        }
    }
}