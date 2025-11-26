using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ABB.Application.EntriPembayaranBanks.Queries
{
    public class GetAllEntriPembayaranBankTempQuery : IRequest<List<EntriPembayaranBankTempDto>> 
    {
        // TAMBAHKAN PROPERTI INI
        public string SearchKeyword { get; set; }
        public string NoVoucher { get; set; }
    }

    public class GetAllEntriPembayaranBankTempQueryHandler : IRequestHandler<GetAllEntriPembayaranBankTempQuery, List<EntriPembayaranBankTempDto>>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetAllEntriPembayaranBankTempQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EntriPembayaranBankTempDto>> Handle(GetAllEntriPembayaranBankTempQuery request, CancellationToken cancellationToken)
        {
            // Ambil data dasar
            var query = _context.EntriPembayaranBankTemp.AsQueryable();

            // TAMBAHKAN LOGIKA FILTER DI SINI
            if (!string.IsNullOrEmpty(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.ToLower();
                query = query.Where(pb => 
                    pb.NoVoucher.ToLower().Contains(keyword) ||
                    pb.KodeAkun.ToLower().Contains(keyword)
                );
            }

            if (!string.IsNullOrEmpty(request.NoVoucher))
            {
                query = query.Where(pb => pb.NoVoucher == request.NoVoucher);
            }
            // ------------------------------------

            return await query
            .Select(pb => new EntriPembayaranBankTempDto
            {
                // Anda harus memetakan SEMUA properti DTO Anda secara manual di sini
                // (Saya tebak nama propertinya, sesuaikan jika salah)
                NoVoucher = pb.NoVoucher,
                No = pb.No,
                FlagPembayaran = pb.FlagPembayaran,
                NoNota4 = pb.NoNota4,
                KodeAkun = pb.KodeAkun,
                KodeMataUang = pb.KodeMataUang,
                TotalBayar = pb.TotalBayar,
                TotalDlmRupiah = pb.TotalDlmRupiah,
                DebetKredit = pb.DebetKredit,
                Kurs = pb.Kurs,
                // ... (tambahkan properti DTO lain jika ada) ...

                // Dan tambahkan logika kalkulasi baru kita
                TotalBayarCalculated = (pb.DebetKredit == "K" ? -pb.TotalBayar : pb.TotalBayar) ?? 0
            })
            .ToListAsync(cancellationToken);
        }
    }
}