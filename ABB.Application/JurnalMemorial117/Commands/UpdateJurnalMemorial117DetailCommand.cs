using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
// HAPUS using ABB.Web.Modules.JurnalMemorial117.Models;  <-- INI PENYEBAB ERROR
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Commands
{
    // HAPUS ", IMapFrom<JurnalMemorial117ViewModel>" DARI SINI
    public class UpdateJurnalMemorial117DetailCommand : IRequest<Unit> 
    {
        public string NoVoucher { get; set; }
        public int No { get; set; }
        public string KodeAkun { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiKredit { get; set; }
        public string KodeUserUpdate { get; set; }

        // HAPUS FUNCTION Mapping() DARI SINI.
        // Mapping sudah diatur di file ViewModel (di project Web), itu sudah cukup.
    }

    public class UpdateJurnalMemorial117DetailCommandHandler : IRequestHandler<UpdateJurnalMemorial117DetailCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public UpdateJurnalMemorial117DetailCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateJurnalMemorial117DetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.JurnalMemorial117Detail
                .FirstOrDefaultAsync(x => x.NoVoucher == request.NoVoucher && x.No == request.No, cancellationToken);

            if (entity == null) throw new Exception("Data Detail tidak ditemukan.");

            entity.KodeAkun = request.KodeAkun;
            entity.NoNota = request.NoNota;
            entity.KodeMataUang = request.KodeMataUang;
            entity.NilaiDebet = request.NilaiDebet;
            entity.NilaiKredit = request.NilaiKredit;
            
            entity.KodeUserUpdate = request.KodeUserUpdate;
            entity.TanggalUserUpdate = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}