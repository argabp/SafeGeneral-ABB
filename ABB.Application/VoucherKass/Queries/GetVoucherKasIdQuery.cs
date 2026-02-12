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
        private readonly UserManager<AppUser> _userManager;

        public GetVoucherKasByIdQueryHandler(IDbContextPstNota context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<VoucherKasDto> Handle(GetVoucherKasByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. FILTERING AWAL
            var queryDasar = _context.VoucherKas.AsNoTracking();

            if (request.Id > 0)
            {
                // Kalau ada ID, cari pakai ID
                queryDasar = queryDasar.Where(x => x.Id == request.Id);
            }
            else
            {
                // Kalau ID 0, cari pakai NoVoucher (di-Trim biar aman dari spasi)
                // Pastikan request.NoVoucher tidak null sebelum di-Trim
                var noVoucherCari = request.NoVoucher != null ? request.NoVoucher.Trim() : "";
                queryDasar = queryDasar.Where(x => x.NoVoucher == noVoucherCari);
            }

            // 2. JOIN & SELECT
            var result = await (
                    from vb in queryDasar 
                    
                    // Gunakan LEFT JOIN ke KasBank
                    join kb in _context.KasBank 
                        on vb.KodeAkun equals kb.NoPerkiraan into kbJoin
                    from kb in kbJoin.DefaultIfEmpty()
                    
                    // Gunakan LEFT JOIN ke MataUang
                    join mu in _context.MataUang
                        on vb.KodeMataUang equals mu.kd_mtu into muJoin
                    from mu in muJoin.DefaultIfEmpty()

                    join cb in _context.Cabang
                        on vb.KodeCabang equals cb.kd_cb into cbJoin
                    from cb in cbJoin.DefaultIfEmpty()

                    // [PENTING] JANGAN ADA WHERE LAGI DI SINI
                    // Karena queryDasar sudah memfilter data yang benar

                    select new VoucherKasDto
                    {
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
                        FlagPosting = vb.FlagPosting ?? false,
                        FlagFinal = vb.FlagFinal ?? false,
                        JenisPembayaran = vb.JenisPembayaran,
                        TanggalInput = vb.TanggalInput,
                        TanggalUpdate = vb.TanggalUpdate,
                        FlagSementara = vb.FlagSementara ?? false, 
                        NoVoucherSementara = vb.NoVoucherSementara,
                        
                        KodeUserInput = vb.KodeUserInput,
                        KodeUserUpdate = vb.KodeUserUpdate,

                        // Handle Null kalau Join tidak ketemu
                        NamaKas = kb != null ? kb.Keterangan : null,
                        NamaMataUang = mu != null ? mu.symbol : null,
                        DetailMataUang = mu != null ? mu.nm_mtu : null,

                        kt = cb != null ? cb.kt : null
                    }
            ).FirstOrDefaultAsync(cancellationToken);

            if (result == null) return null;

            // 3. LOOKUP USERNAME
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