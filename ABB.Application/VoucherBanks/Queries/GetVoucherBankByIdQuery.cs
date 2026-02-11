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
        public long Id { get; set; }
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
            // 1. FILTERING DINAMIS (Logic Perbaikan Utama)
            var queryDasar = _context.VoucherBank.AsNoTracking();

            if (request.Id > 0)
            {
                // Kalau ada ID, cari pakai ID (Prioritas)
                queryDasar = queryDasar.Where(x => x.Id == request.Id);
            }
            else
            {
                // Kalau ID 0, cari pakai NoVoucher (dengan Trim biar aman)
                var noVoucherCari = request.NoVoucher != null ? request.NoVoucher.Trim() : "";
                queryDasar = queryDasar.Where(x => x.NoVoucher == noVoucherCari);
            }

            // 2. JOIN & PROJECTION
            var result = await (
                                from vb in queryDasar
                                
                                // Gunakan LEFT JOIN ke KasBank (via KodeBank)
                                join kb in _context.KasBank 
                                    on vb.KodeBank equals kb.Kode into kbJoin
                                from kb in kbJoin.DefaultIfEmpty()

                                // Gunakan LEFT JOIN ke MataUang
                                join mu in _context.MataUang
                                    on vb.KodeMataUang equals mu.kd_mtu into muJoin
                                from mu in muJoin.DefaultIfEmpty()

                                // [PENTING] HAPUS WHERE DI SINI (Karena sudah di queryDasar)

                                select new VoucherBankDto
                                {
                                    Id = vb.Id,
                                    NoVoucher = vb.NoVoucher,
                                    KodeCabang = vb.KodeCabang,
                                    JenisVoucher = vb.JenisVoucher,
                                    DebetKredit = vb.DebetKredit,
                                    KodeAkun = vb.KodeAkun,
                                    
                                    DiterimaDari = vb.DiterimaDari, // Sesuai entity bank
                                    
                                    TanggalVoucher = vb.TanggalVoucher,
                                    KodeMataUang = vb.KodeMataUang,
                                    TotalVoucher = vb.TotalVoucher,
                                    TotalDalamRupiah = vb.TotalDalamRupiah,
                                    KeteranganVoucher = vb.KeteranganVoucher,
                                    FlagPosting = vb.FlagPosting ?? false, // Handle null bool
                                     FlagFinal = vb.FlagFinal ?? false,
                                    KodeBank = vb.KodeBank,
                                    NoBank = vb.NoBank,
                                    
                                    JenisPembayaran = vb.JenisPembayaran,
                                    TanggalInput = vb.TanggalInput,
                                    TanggalUpdate = vb.TanggalUpdate,
                                    FlagSementara = vb.FlagSementara ?? false, 
                                    NoVoucherSementara = vb.NoVoucherSementara,
                       
                                    KodeUserInput = vb.KodeUserInput,
                                    KodeUserUpdate = vb.KodeUserUpdate,

                                    // Handle Null jika data join tidak ditemukan
                                    NamaBank = kb != null ? kb.Keterangan : null,
                                    NamaMataUang = mu != null ? mu.symbol : null,
                                    DetailMataUang = mu != null ? mu.nm_mtu : null
                                })
                                .FirstOrDefaultAsync(cancellationToken);

            if (result == null) return null;

            // 3. LOOKUP USERNAME (Tetap Sama)
            if (!string.IsNullOrEmpty(result.KodeUserInput))
            {
                var userIn = await _userManager.FindByIdAsync(result.KodeUserInput);
                if (userIn != null) result.KodeUserInput = userIn.UserName;
            }

            if (!string.IsNullOrEmpty(result.KodeUserUpdate))
            {
                var userUp = await _userManager.FindByIdAsync(result.KodeUserUpdate);
                if (userUp != null) result.KodeUserUpdate = userUp.UserName;
            }
            
            return result;
        }
    }
}