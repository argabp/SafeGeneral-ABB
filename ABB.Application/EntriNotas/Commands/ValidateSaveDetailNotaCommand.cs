using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.EntriNotas.Commands
{
    public class ValidateSaveDetailNotaCommand : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public string no_pol { get; set; }
        public decimal nilai_nt { get; set; }
        public decimal nilai_ang { get; set; }
    }

    public class ValidateSaveDetailNotaCommandHandler : IRequestHandler<ValidateSaveDetailNotaCommand, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ValidateSaveDetailNotaCommandHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> Handle(ValidateSaveDetailNotaCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.QueryProc<string>("spe_uw03e_03", 
                new { request.no_pol, request.nilai_nt, request.nilai_ang })).FirstOrDefault();
        }
    }
}