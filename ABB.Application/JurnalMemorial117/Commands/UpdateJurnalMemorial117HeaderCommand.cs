using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.JurnalMemorial117.Queries;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.JurnalMemorial117.Commands
{
    // Mapping dari DTO ke Command Update
    public class UpdateJurnalMemorial117HeaderCommand : IRequest<Unit>, IMapFrom<JurnalMemorial117Dto>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
        public string KodeUserUpdate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JurnalMemorial117Dto, UpdateJurnalMemorial117HeaderCommand>();
        }
    }

    public class UpdateJurnalMemorial117HeaderCommandHandler : IRequestHandler<UpdateJurnalMemorial117HeaderCommand, Unit>
    {
        private readonly IDbContextPstNota _context;

        public UpdateJurnalMemorial117HeaderCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateJurnalMemorial117HeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.JurnalMemorial117
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