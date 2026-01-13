using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VoucherKasEntity = ABB.Domain.Entities.VoucherKas;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Application.VoucherKass.Queries
{
    public class GetVoucherKasByIdQuery : IRequest<VoucherKasDto>
    {
        public string NoVoucher { get; set; }
    }

    public class GetVoucherKasByIdQueryHandler : IRequestHandler<GetVoucherKasByIdQuery, VoucherKasDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        // [TAMBAHAN] Inject UserManager untuk cari nama user
        private readonly UserManager<AppUser> _userManager;

        public GetVoucherKasByIdQueryHandler(IDbContextPstNota context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<VoucherKasDto> Handle(GetVoucherKasByIdQuery request, CancellationToken cancellationToken)
        {
           var result = await (
                 from vb in _context.VoucherKas
                 join kb in _context.KasBank 
                     on vb.KodeAkun equals kb.NoPerkiraan
                 join mu in _context.MataUang
                     on vb.KodeMataUang equals mu.kd_mtu
                 where vb.NoVoucher == request.NoVoucher
                 select new VoucherKasDto
                {
                    NoVoucher = vb.NoVoucher,
                    KodeCabang = vb.KodeCabang,
                    JenisVoucher = vb.JenisVoucher,
                    DebetKredit = vb.DebetKredit,
                    KodeAkun = vb.KodeAkun,
                    DibayarKepada = vb.DibayarKepada,
                    TanggalVoucher = vb.TanggalVoucher,
                    KodeMataUang = vb.KodeMataUang,
                    TotalVoucher = vb.TotalVoucher,
                    TotalDalamRupiah = vb.TotalDalamRupiah,
                    KeteranganVoucher = vb.KeteranganVoucher,
                    FlagPosting = (bool)vb.FlagPosting,
                    JenisPembayaran = vb.JenisPembayaran,
                    TanggalInput = vb.TanggalInput,
                    TanggalUpdate = vb.TanggalUpdate,
                    
                    // Ambil ID-nya dulu (Nanti kita ubah jadi Nama di bawah)
                    KodeUserInput = vb.KodeUserInput,
                    KodeUserUpdate = vb.KodeUserUpdate,

                    // dari KasBank
                    NamaKas = kb.Keterangan,

                    // dari MataUang
                    NamaMataUang = mu.symbol,
                    DetailMataUang = mu.nm_mtu
                }
            ).FirstOrDefaultAsync(cancellationToken);
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