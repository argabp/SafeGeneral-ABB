using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ABB.Application.InquiryNotaProduksis.Commands
{
    public class SaveKeteranganOSCommand : IRequest<bool>
    {
        public int IdNota { get; set; }
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
        var lastId = await _context.KeteranganProduksi
            .OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var newId = lastId + 1;

        var entity = new KeteranganProduksi
        {
            Id = newId,
            IdNota = request.IdNota,   // relasi ke Produksi
            NoNota = request.NoNota,
            Tanggal = request.Tanggal,
            Keterangan = request.Keterangan
        };

        _context.KeteranganProduksi.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
}
