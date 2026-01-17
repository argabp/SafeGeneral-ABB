using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            // 1. Ambil Data Header dari Database
            var entity = await _context.HeaderPenyelesaianUtang
                .AsNoTracking() // Optional: Biar lebih ringan
                .FirstOrDefaultAsync(h => h.KodeCabang == request.KodeCabang && h.NomorBukti == request.NomorBukti, cancellationToken);
            
            if (entity == null) return null;

            // 2. Mapping Entity ke DTO
            var dto = _mapper.Map<HeaderPenyelesaianUtangDto>(entity);

            // 3. LOGIC CARI NAMA USER (Sama seperti Voucher)
            
            // Cek User Input
            if (!string.IsNullOrEmpty(dto.KodeUserInput))
            {
                var userIn = await _userManager.FindByIdAsync(dto.KodeUserInput);
                if (userIn != null) 
                {
                    dto.KodeUserInput = userIn.UserName; // Ganti ID jadi Nama
                }
            }

            // Cek User Update
            if (!string.IsNullOrEmpty(dto.KodeUserUpdate))
            {
                var userUp = await _userManager.FindByIdAsync(dto.KodeUserUpdate);
                if (userUp != null) 
                {
                    dto.KodeUserUpdate = userUp.UserName; // Ganti ID jadi Nama
                }
            }

            return dto;
        }
    }
}