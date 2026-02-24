using System.Linq; // <--- WAJIB ADA buat pake Query Pattern (from, where, join)
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    public class GetHeaderPenyelesaianUtangByIdQuery : IRequest<HeaderPenyelesaianUtangDto>
    {
        public string KodeCabang { get; set; }
        public string NomorBukti { get; set; }
    }

    public class GetHeaderPenyelesaianUtangByIdQueryHandler : IRequestHandler<GetHeaderPenyelesaianUtangByIdQuery, HeaderPenyelesaianUtangDto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        // [TAMBAHAN] Inject UserManager untuk cari nama user
        private readonly UserManager<AppUser> _userManager;

        public GetHeaderPenyelesaianUtangByIdQueryHandler(IDbContextPstNota context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<HeaderPenyelesaianUtangDto> Handle(GetHeaderPenyelesaianUtangByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Definisikan Query dengan Join
            var query = from h in _context.HeaderPenyelesaianUtang.AsNoTracking()
                        where h.KodeCabang == request.KodeCabang && h.NomorBukti == request.NomorBukti

                        // Join ke MataUang (Left Join)
                        join mu in _context.MataUang on h.MataUang equals mu.kd_mtu into muJoin
                        from mu in muJoin.DefaultIfEmpty()

                        // Join ke Cabang (Left Join)
                        join cb in _context.Cabang on h.KodeCabang equals cb.kd_cb into cbJoin
                        from cb in cbJoin.DefaultIfEmpty()

                        select new HeaderPenyelesaianUtangDto
                        {
                            KodeCabang = h.KodeCabang,
                            NomorBukti = h.NomorBukti,
                            JenisPenyelesaian = h.JenisPenyelesaian,
                            KodeVoucherAcc = h.KodeVoucherAcc,
                            Tanggal = h.Tanggal,
                            MataUang = h.MataUang,
                            TotalOrg = h.TotalOrg,
                            TotalRp = h.TotalRp,
                            DebetKredit = h.DebetKredit,
                            Keterangan = h.Keterangan,
                            KodeAkun = h.KodeAkun,
                            TanggalInput = h.TanggalInput,
                            TanggalUpdate = h.TanggalUpdate,
                            FlagPosting = h.FlagPosting ?? false,
                            FlagFinal = h.FlagFinal ?? false,
                            
                            // Isi properti metadata dari hasil Join
                            NamaMataUang = mu != null ? mu.symbol : null,  // Jadi: Rp
                            DetailMataUang = mu != null ? mu.nm_mtu : null, // Jadi: Rupiah
                            kt = cb != null ? cb.kt : "Jakarta",          // Contoh: Surabaya
                            
                            KodeUserInput = h.KodeUserInput,
                            KodeUserUpdate = h.KodeUserUpdate
                        };

            var dto = await query.FirstOrDefaultAsync(cancellationToken);

            if (dto == null) return null;

            // 2. Lookup Username
            if (!string.IsNullOrEmpty(dto.KodeUserInput))
            {
                var userIn = await _userManager.FindByIdAsync(dto.KodeUserInput);
                if (userIn != null) dto.KodeUserInput = userIn.UserName; 
            }

            if (!string.IsNullOrEmpty(dto.KodeUserUpdate))
            {
                var userUp = await _userManager.FindByIdAsync(dto.KodeUserUpdate);
                if (userUp != null) dto.KodeUserUpdate = userUp.UserName; 
            }

            return dto;
        }
    }
}