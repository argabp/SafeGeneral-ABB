using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.InquiryNotaProduksis.Commands
{
    public class SaveKeteranganOSCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string NoNota { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
    }

   public class SaveKeteranganOSCommandHandler
    : IRequestHandler<SaveKeteranganOSCommand, bool>
{
    private readonly IDbContextPstNota _context;

    public SaveKeteranganOSCommandHandler(IDbContextPstNota context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        SaveKeteranganOSCommand request,
        CancellationToken cancellationToken)
    {
        // 🔎 cari berdasarkan ID PRODUKSI
        var entity = await _context.KeteranganProduksi
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            // ➕ INSERT
            entity = new KeteranganProduksi
            {
                Id = request.Id,                 // ✅ WAJIB (bukan identity)
                NoNota = request.NoNota,
                Tanggal = request.Tanggal,
                Keterangan = request.Keterangan
            };

            _context.KeteranganProduksi.Add(entity);
        }
        else
        {
            // 🔁 UPDATE
            entity.NoNota = request.NoNota;
            entity.Tanggal = request.Tanggal;
            entity.Keterangan = request.Keterangan;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
}
