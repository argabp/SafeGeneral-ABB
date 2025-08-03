using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNameAandAddressObligeeQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_rk_obl { get; set; }
    }

    public class GenerateNameAandAddressObligeeQueryHandler : IRequestHandler<GenerateNameAandAddressObligeeQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenerateNameAandAddressObligeeQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(GenerateNameAandAddressObligeeQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw02e_04b", 
                new { request.kd_cb, request.kd_rk_obl })).FirstOrDefault();
        }
    }
}