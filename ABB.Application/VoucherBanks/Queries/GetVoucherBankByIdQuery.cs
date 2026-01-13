using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VoucherBankEntity = ABB.Domain.Entities.VoucherBank;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

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
         private readonly UserManager<AppUser> _userManager;

        public GetVoucherBankByIdQueryHandler(IDbContextPstNota context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
             _userManager = userManager;
        }

        public async Task<VoucherBankDto> Handle(GetVoucherBankByIdQuery request, CancellationToken cancellationToken)
        {
            // Gunakan LINQ untuk menggabungkan (JOIN) dua tabel
            var result = await (
                                // 1. SUMBER DATA UTAMA (WAJIB DI PALING ATAS)
                                from vb in _context.VoucherBank
                                
                                // 2. Join ke Tabel Lain (KasBank & MataUang)
                                join kb in _context.KasBank 
                                    on vb.KodeBank equals kb.Kode
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
                                    TanggalUpdate = vb.TanggalUpdate,
                                    // Ambil ID-nya dulu (Nanti kita ubah jadi Nama di bawah)
                                    KodeUserInput = vb.KodeUserInput,
                                    KodeUserUpdate = vb.KodeUserUpdate,

                                    // Ambil Keterangan dari KasBank (kb) dan isi ke NamaBank
                                    NamaBank = kb.Keterangan,
                                    NamaMataUang = mu.symbol,
                                    DetailMataUang = mu.nm_mtu
                                })
                                .FirstOrDefaultAsync(cancellationToken);
                                if (result == null) return null;

                                // LANGKAH 2: Ambil Nama User via UserManager (Manual Lookup)
                                // Ini aman karena UserManager biasanya terkoneksi ke Database Pusat/Identity
                                
                                // Cek User Input
                                if (!string.IsNullOrEmpty(result.KodeUserInput))
                                {
                                    var userIn = await _userManager.FindByIdAsync(result.KodeUserInput);
                                    if (userIn != null) 
                                    {
                                        result.KodeUserInput = userIn.UserName; // Ganti ID jadi Nama
                                    }
                                }

                                // Cek User Update
                                if (!string.IsNullOrEmpty(result.KodeUserUpdate))
                                {
                                    var userUp = await _userManager.FindByIdAsync(result.KodeUserUpdate);
                                    if (userUp != null) 
                                    {
                                        result.KodeUserUpdate = userUp.UserName; // Ganti ID jadi Nama
                                    }
                                }
            return result;
        }
    }
}