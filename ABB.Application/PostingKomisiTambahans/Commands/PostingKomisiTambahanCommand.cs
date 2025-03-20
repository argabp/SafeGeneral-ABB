using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Common.Services;
using ABB.Application.PostingKomisiTambahans.Queries;
using ABB.Application.PostingPolicies.Queries;
using MediatR;

namespace ABB.Application.PostingKomisiTambahans.Commands
{
    public class PostingKomisiTambahanCommand : IRequest
    {
        public string DatabaseName { get; set; }

        public List<PostingKomisiTambahanDto> Data { get; set; }
    }

    public class PostingKomisiTambahanCommandHandler : IRequestHandler<PostingKomisiTambahanCommand>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ICurrentUserService _userService;

        public PostingKomisiTambahanCommandHandler(IDbConnectionFactory connectionFactory, ICurrentUserService userService)
        {
            _connectionFactory = connectionFactory;
            _userService = userService;
        }

        public async Task<Unit> Handle(PostingKomisiTambahanCommand request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            foreach (var data in request.Data)
            {
                await _connectionFactory.QueryProc("spp_fn01p_02",
                    new
                    {
                        data.jns_sb_nt, data.kd_cb, data.jns_tr, data.jns_nt_msk, data.kd_thn,
                        data.kd_bln, data.no_nt_msk, data.jns_nt_kel, data.no_nt_kel,
                        tgl_posting = DateTime.Now, kd_usr_posting = _userService.UserName
                    });
            }
            
            return Unit.Value;
        }
    }
}