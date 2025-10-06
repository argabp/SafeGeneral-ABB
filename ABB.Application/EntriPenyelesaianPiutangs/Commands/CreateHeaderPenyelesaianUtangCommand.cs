using System;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;
using MediatR;
using HeaderPenyelesaianUtangEntity = ABB.Domain.Entities.HeaderPenyelesaianUtang;

namespace ABB.Application.EntriPenyelesaianPiutangs.Commands
{
    // "Surat Perintah" untuk membuat header baru
    public class CreateHeaderPenyelesaianUtangCommand : IRequest<string>, IMapFrom<HeaderPenyelesaianUtang>

    {
        public string KodeCabang { get; set; }
        public string JenisPenyelesaian { get; set; }
        public string NomorBukti { get; set; }
        public string KodeVoucherAcc { get; set; }
        public DateTime? Tanggal { get; set; }
        public string MataUang { get; set; }
        public decimal? TotalOrg { get; set; }
        public decimal? TotalRp { get; set; }
        public string Keterangan { get; set; }
        public string KodeAkun { get; set; }
        public string KodeUserInput { get; set; } // Untuk audit trail
        public bool? FlagPosting { get; set; }
       public DateTime? TanggalInput { get; set; }
    }

    // "Petugas Pelaksana" untuk perintah di atas
    public class CreateHeaderPenyelesaianUtangCommandHandler : IRequestHandler<CreateHeaderPenyelesaianUtangCommand, string>
    {
        private readonly IDbContextPstNota _context;
        

        public CreateHeaderPenyelesaianUtangCommandHandler(IDbContextPstNota context)
        {
            _context = context;
           
        }

        public async Task<string> Handle(CreateHeaderPenyelesaianUtangCommand request, CancellationToken cancellationToken)
        {
            // Mapping dari Command ke Entity
            var entity = new HeaderPenyelesaianUtangEntity 
            {

                // Isi data audit trail
               TanggalInput = DateTime.Now,
               KodeUserInput = request.KodeUserInput, // Pastikan ini dikirim dari Controller
               FlagPosting = false, // Nilai default

               KodeCabang = request.KodeCabang,
               JenisPenyelesaian = request.JenisPenyelesaian,
               NomorBukti = request.NomorBukti,
               KodeVoucherAcc = request.KodeVoucherAcc,
               Tanggal = request.Tanggal,
               MataUang = request.MataUang,
               TotalOrg = request.TotalOrg,
               TotalRp = request.TotalRp,
               Keterangan = request.Keterangan,
               KodeAkun = request.KodeAkun

            };
            _context.HeaderPenyelesaianUtang.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // Kembalikan nomor bukti yang baru saja disimpan
            return entity.NomorBukti;
        }
    }
}