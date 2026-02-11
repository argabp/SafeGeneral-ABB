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
        public long Id { get; set; }
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
            // [PERBAIKAN 1] LOGIC FILTER DINAMIS
            // Kita siapkan dulu query dasarnya
            var queryDasar = _context.VoucherKas.AsNoTracking();

            if (request.Id > 0)
            {
                // Prioritas 1: Filter pakai ID (Primary Key)
                queryDasar = queryDasar.Where(x => x.Id == request.Id);
            }
            else
            {
                // Prioritas 2: Filter pakai NoVoucher (Backup)
                queryDasar = queryDasar.Where(x => x.NoVoucher == request.NoVoucher);
            }

            // [PERBAIKAN 2] JOIN & SELECT
            // Perhatikan: 'from vb in queryDasar' (bukan _context.VoucherKas lagi)
            var result = await (
                    from vb in queryDasar 
                    join kb in _context.KasBank 
                        on vb.KodeAkun equals kb.NoPerkiraan
                    join mu in _context.MataUang
                        on vb.KodeMataUang equals mu.kd_mtu
                    where vb.Id == request.Id
                    select new VoucherKasDto
                    {
                        // [PENTING!] Masukkan ID ke DTO
                        Id = vb.Id, 

                        NoVoucher = vb.NoVoucher,
                        KodeCabang = vb.KodeCabang,
                        JenisVoucher = vb.JenisVoucher,
                        DebetKredit = vb.DebetKredit,
                        KodeAkun = vb.KodeAkun,
                        KodeKas = vb.KodeKas,
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
                        FlagSementara = vb.FlagSementara ?? false, 
                        NoVoucherSementara = vb.NoVoucherSementara,
                        
                        KodeUserInput = vb.KodeUserInput,
                        KodeUserUpdate = vb.KodeUserUpdate,

                        NamaKas = kb.Keterangan,
                        NamaMataUang = mu.symbol,
                        DetailMataUang = mu.nm_mtu
                    }
            ).FirstOrDefaultAsync(cancellationToken);

            if (result == null) return null;

            // LANGKAH 3: Lookup User Identity (Tetap Sama)
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