using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MutasiKlaims.Commands
{
    public class InsertMutasiKlaimCommand : IRequest<InsertMutasiKlaimDto>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string tipe_mts { get; set; }

        public string kd_mtu { get; set; }

        public string flag_konv { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InsertMutasiKlaimCommand, MutasiKlaim>();
        }
    }

    public class InsertMutasiKlaimCommandHandler : IRequestHandler<InsertMutasiKlaimCommand, InsertMutasiKlaimDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;
        private readonly ILogger<InsertMutasiKlaimCommandHandler> _logger;

        public InsertMutasiKlaimCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService,
            ILogger<InsertMutasiKlaimCommandHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
            _logger = logger;
        }

        public async Task<InsertMutasiKlaimDto> Handle(InsertMutasiKlaimCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var result = (await _connectionFactory.QueryProc<InsertMutasiKlaimDto>("spp_cl02e_03", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.kd_thn,
                    request.no_kl, request.tipe_mts, request.kd_mtu, request.flag_konv,
                    kd_usr_input = _userService.UserId
                })).FirstOrDefault();

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                throw e.InnerException ?? e;
            }
        }
    }
}