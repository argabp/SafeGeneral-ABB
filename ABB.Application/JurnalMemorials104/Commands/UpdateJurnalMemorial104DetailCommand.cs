using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorials104.Commands
{
    // HAPUS ", IMapFrom<JurnalMemorial104ViewModel>" DARI SINI
    public class UpdateJurnalMemorial104DetailCommand : IRequest<Unit> 
    {
        public string NoVoucher { get; set; }
        public int No { get; set; }
        public string KodeAkun { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiDebetRp { get; set; }
        public decimal? NilaiKredit { get; set; }
        public decimal? NilaiKreditRp { get; set; }
        public string KodeUserUpdate { get; set; }
        public string KeteranganDetail { get; set; }

        // HAPUS FUNCTION Mapping() DARI SINI.
        // Mapping sudah diatur di file ViewModel (di project Web), itu sudah cukup.
    }

    public class UpdateJurnalMemorial104DetailCommandHandler : IRequestHandler<UpdateJurnalMemorial104DetailCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public UpdateJurnalMemorial104DetailCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateJurnalMemorial104DetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.DetailJurnalMemorial104
                .FirstOrDefaultAsync(x => x.NoVoucher == request.NoVoucher && x.No == request.No, cancellationToken);

            if (entity == null) throw new Exception("Data Detail tidak ditemukan.");

            entity.KodeAkun = request.KodeAkun;
            entity.NoNota = request.NoNota;
            entity.KodeMataUang = request.KodeMataUang;
            entity.NilaiDebet = request.NilaiDebet;
            entity.NilaiDebetRp = request.NilaiDebetRp;
            entity.NilaiKredit = request.NilaiKredit;
            entity.NilaiKreditRp = request.NilaiKreditRp;
            entity.KodeUserUpdate = request.KodeUserUpdate;
            entity.TanggalUserUpdate = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}