using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;
using AutoMapper;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    public class UpdateHeaderPenyelesaianUtangCommand : IRequest, IMapFrom<HeaderPenyelesaianUtangDto>
    {
        // Semua properti yang bisa di-update
        public string KodeCabang { get; set; }
        public string NomorBukti { get; set; }
        public string JenisPenyelesaian { get; set; }
        public string KodeVoucherAcc { get; set; }
        public DateTime? Tanggal { get; set; }
        public string MataUang { get; set; }
        public decimal? TotalOrg { get; set; }
        public decimal? TotalRp { get; set; }
        public string DebetKredit { get; set; }
        public string Keterangan { get; set; }
        public string KodeAkun { get; set; }
        public string KodeUserUpdate { get; set; }
        public DateTime? TanggalUpdate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<HeaderPenyelesaianUtangDto, UpdateHeaderPenyelesaianUtangCommand>();
        }
    }

    public class UpdateHeaderPenyelesaianUtangCommandHandler : IRequestHandler<UpdateHeaderPenyelesaianUtangCommand>
    {
        private readonly IDbContextPstNota _context;
        public UpdateHeaderPenyelesaianUtangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateHeaderPenyelesaianUtangCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.HeaderPenyelesaianUtang
                .FindAsync(new object[] { request.KodeCabang, request.NomorBukti }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(HeaderPenyelesaianUtang), request.NomorBukti);
            }

            // Update properti dari request
            entity.JenisPenyelesaian = request.JenisPenyelesaian;
            entity.KodeVoucherAcc = request.KodeVoucherAcc;
            entity.Tanggal = request.Tanggal;
            entity.MataUang = request.MataUang;
            entity.TotalOrg = request.TotalOrg;
            entity.TotalRp = request.TotalRp;
            entity.DebetKredit = request.DebetKredit;
            entity.Keterangan = request.Keterangan;
            entity.KodeAkun = request.KodeAkun;
            entity.KodeUserUpdate = request.KodeUserUpdate;
            entity.TanggalUpdate = request.TanggalUpdate;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}