using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Application.ProfilRisikoHartaBendas.Queries
{
    public class GetProfilRisikoHartaBendasQuery : IRequest<string>
    {
        public string thn_uw { get; set; }
    }
    
    public class GetProfilRisikoHartaBendasQueryHandler : IRequestHandler<GetProfilRisikoHartaBendasQuery, string>
    {
        private readonly IDbConnectionCSM _db;
        private readonly ILogger<GetProfilRisikoHartaBendasQueryHandler> _logger;

        public GetProfilRisikoHartaBendasQueryHandler(IDbConnectionCSM db, ILogger<GetProfilRisikoHartaBendasQueryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }
    
        public async Task<string> Handle(GetProfilRisikoHartaBendasQuery request, CancellationToken cancellationToken)
        {
            List<dynamic> data = new List<dynamic>();
            try
            {
                data = (await _db.QueryProc<dynamic>("spr_teknik_sipetir01",
                    new
                    {
                        thn_uw = Convert.ToDecimal(request.thn_uw)
                    })).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw;
            }

            return JsonConvert.SerializeObject(data);
        }
    }
}