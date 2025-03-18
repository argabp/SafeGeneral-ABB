using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetJenisSorQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetJenisSorQueryHandler : IRequestHandler<GetJenisSorQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetJenisSorQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetJenisSorQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<DropdownOptionDto>("SELECT kd_jns_sor Value, nm_jns_sor Text FROM rf18")).ToList();
        }
    }
}