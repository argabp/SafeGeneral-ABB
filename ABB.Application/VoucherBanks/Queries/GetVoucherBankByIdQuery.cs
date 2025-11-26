using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;

namespace ABB.Application.VoucherBanks.Queries
{
    public class GetVoucherBankByIdQuery : IRequest<VoucherBankDto>
    {
        public string NoVoucher { get; set; }
    }

    public class GetVoucherBankByIdQueryHandler : IRequestHandler<GetVoucherBankByIdQuery, VoucherBankDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;

        public GetVoucherBankByIdQueryHandler(IDbContextPstNota context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VoucherBankDto> Handle(GetVoucherBankByIdQuery request, CancellationToken cancellationToken)
        {
            // Gunakan LINQ untuk menggabungkan (JOIN) dua tabel
            var result = await (from vb in _context.VoucherBank
                                join kb in _context.KasBank on vb.KodeBank equals kb.Kode
                                 join mu in _context.MataUang
                                    on vb.KodeMataUang equals mu.kd_mtu
                                where vb.NoVoucher == request.NoVoucher
                                select new VoucherBankDto
                                {
                                    // Salin semua properti dari VoucherBank (vb)
                                    NoVoucher = vb.NoVoucher,
                                    KodeCabang = vb.KodeCabang,
                                    JenisVoucher = vb.JenisVoucher,
                                    DebetKredit = vb.DebetKredit,
                                    KodeAkun = vb.KodeAkun,
                                    DiterimaDari = vb.DiterimaDari,
                                    TanggalVoucher = vb.TanggalVoucher,
                                    KodeMataUang = vb.KodeMataUang,
                                    TotalVoucher = vb.TotalVoucher,
                                    TotalDalamRupiah = vb.TotalDalamRupiah,
                                    KeteranganVoucher = vb.KeteranganVoucher,
                                    FlagPosting = (bool)vb.FlagPosting,
                                    KodeBank = vb.KodeBank,
                                    NoBank = vb.NoBank,
                                    JenisPembayaran = vb.JenisPembayaran,
                                    TanggalInput = vb.TanggalInput,
                                    KodeUserInput = vb.KodeUserInput,
                                    TanggalUpdate = vb.TanggalUpdate,
                                    KodeUserUpdate = vb.KodeUserUpdate,

                                    // Ambil Keterangan dari KasBank (kb) dan isi ke NamaBank
                                    NamaBank = kb.Keterangan,
                                    NamaMataUang = mu.symbol,
                                    DetailMataUang = mu.nm_mtu
                                })
                                .FirstOrDefaultAsync(cancellationToken);
            
            return result;
        }
    }
}