using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    // Command untuk menghapus Header beserta Detail-nya
    public class DeleteHeaderPenyelesaianUtangCommand : IRequest<bool>
    {
        public string KodeCabang { get; set; }
        public string NomorBukti { get; set; }
    }

    public class DeleteHeaderPenyelesaianUtangCommandHandler : IRequestHandler<DeleteHeaderPenyelesaianUtangCommand, bool>
    {
        private readonly IDbContextPstNota _context;

        public DeleteHeaderPenyelesaianUtangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteHeaderPenyelesaianUtangCommand request, CancellationToken cancellationToken)
        {
            // 1. Cari Header yang akan dihapus (berdasarkan Composite Key: KodeCabang + NomorBukti)
            var header = await _context.HeaderPenyelesaianUtang
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NomorBukti == request.NomorBukti, cancellationToken);

            if (header == null)
            {
                // Data header tidak ditemukan
                return false; 
                // Atau bisa throw exception: throw new Exception("Data tidak ditemukan.");
            }

            // 2. Hapus Semua Detail yang Terkait (Cascade Delete Manual)
            // Cari data di tabel DETAIL (bukan Temp) yang punya NoBukti sama
            var details = _context.EntriPenyelesaianPiutang
                .Where(x => x.NoBukti == request.NomorBukti)
                .ToList();

            if (details.Any())
            {
                _context.EntriPenyelesaianPiutang.RemoveRange(details);
            }

            // 3. Hapus juga data di tabel TEMP (jika ada sisa sampah data)
            var tempDetails = _context.EntriPenyelesaianPiutangTemp
                .Where(x => x.NoBukti == request.NomorBukti)
                .ToList();

            if (tempDetails.Any())
            {
                _context.EntriPenyelesaianPiutangTemp.RemoveRange(tempDetails);
            }

            // 4. Hapus Header
            _context.HeaderPenyelesaianUtang.Remove(header);

            // 5. Simpan Perubahan (Commit Transaction)
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}