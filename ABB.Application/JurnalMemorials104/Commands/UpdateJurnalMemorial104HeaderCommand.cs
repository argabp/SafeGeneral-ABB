using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.JurnalMemorials104.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorials104.Commands
{
    // Mapping dari DTO ke Command Update
    public class UpdateJurnalMemorial104HeaderCommand : IRequest<Unit>, IMapFrom<JurnalMemorial104Dto>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
        public string KodeUserUpdate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JurnalMemorial104Dto, UpdateJurnalMemorial104HeaderCommand>();
        }
    }

    public class UpdateJurnalMemorial104HeaderCommandHandler : IRequestHandler<UpdateJurnalMemorial104HeaderCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public UpdateJurnalMemorial104HeaderCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateJurnalMemorial104HeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.JurnalMemorial104
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity == null) throw new Exception("Data Header tidak ditemukan.");

            if (request.Tanggal.HasValue)
                entity.Tanggal = request.Tanggal.Value.ToLocalTime().Date;

            entity.Keterangan = request.Keterangan;
            entity.KodeUserUpdate = request.KodeUserUpdate;
            entity.TanggalUpdate = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}