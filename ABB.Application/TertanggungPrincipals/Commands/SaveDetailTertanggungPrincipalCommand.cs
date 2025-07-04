﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailTertanggungPrincipalCommand : IRequest, IMapFrom<DetailRekanan>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }

        public string? bentukflag { get; set; }

        public string? ktp_nm { get; set; }

        public string? ktp_tempat { get; set; }

        public DateTime? ktp_tgl { get; set; }

        public string? ktp_no { get; set; }

        public string? ktp_alamat { get; set; }

        public string? ktp_normh { get; set; }

        public string? ktp_rtrw { get; set; }

        public string? ktp_kota { get; set; }

        public string? kodepos { get; set; }

        public string? telp { get; set; }

        public string? hp { get; set; }

        public string? wniwna { get; set; }

        public string? wniflag { get; set; }

        public string? wnaflag { get; set; }

        public string? npwp { get; set; }

        public string? kawinflag { get; set; }

        public string? pekerjaanflag { get; set; }

        public string? pekerjaanlain { get; set; }

        public string? jabatan { get; set; }

        public string? perusahaanorang { get; set; }

        public string? usaha { get; set; }

        public string? usahathn { get; set; }

        public string? usahabln { get; set; }

        public string? usahaalamat { get; set; }

        public string? usahakota { get; set; }

        public string? usahakodepos { get; set; }

        public string? usahatelp { get; set; }

        public string? usahatelpext { get; set; }

        public string? usahaflag { get; set; }

        public string? usahahasilflag { get; set; }

        public string? perusahaaninstitusi { get; set; }

        public string? usahainstitusi { get; set; }

        public string? npwpinstitusi { get; set; }

        public string? siupinstitusi { get; set; }

        public string? tdpinstitusi { get; set; }

        public string? hukumhaminstitusi { get; set; }

        public string? kotainstitusi { get; set; }

        public string? kodeposinstitusi { get; set; }

        public string? telpinstitusi { get; set; }

        public string? telpextinstitusi { get; set; }

        public string? wniwnainstitusi { get; set; }

        public string? wniflaginstitusi { get; set; }

        public string? wnaflaginstitusi { get; set; }

        public string? dirinstitusi { get; set; }

        public string? nopolis1 { get; set; }

        public string? jenispolis1 { get; set; }

        public string? asuransipolis1 { get; set; }

        public string? nopolis2 { get; set; }

        public string? jenispolis2 { get; set; }

        public string? asuransipolis2 { get; set; }

        public string? tujuanpolisflag { get; set; }

        public string? tujuanpolislain { get; set; }

        public string? website { get; set; }

        public string? no_fax { get; set; }

        public string? email { get; set; }

        public string? siup { get; set; }

        public decimal? penghasilan { get; set; }

        public string? kelamin { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailTertanggungPrincipalCommand, DetailRekanan>();
        }
    }

    public class SaveDetailTertanggungPrincipalCommandHandler : IRequestHandler<SaveDetailTertanggungPrincipalCommand>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<SaveDetailTertanggungPrincipalCommandHandler> _logger;

        public SaveDetailTertanggungPrincipalCommandHandler(IDbContextFactory contextFactory, IMapper mapper,
            ILogger<SaveDetailTertanggungPrincipalCommandHandler> logger)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(SaveDetailTertanggungPrincipalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbContext = _contextFactory.CreateDbContext(request.DatabaseName);
                var detailRekanan = dbContext.DetailRekanan.FirstOrDefault(w => w.kd_cb == request.kd_cb
                                                                                    && w.kd_grp_rk == request.kd_grp_rk
                                                                                    && w.kd_rk == request.kd_rk);

                if (detailRekanan == null)
                    dbContext.DetailRekanan.Add(_mapper.Map<DetailRekanan>(request));
                else
                    _mapper.Map(request, detailRekanan);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                throw ex;
            }

            return Unit.Value;
        }
    }
}