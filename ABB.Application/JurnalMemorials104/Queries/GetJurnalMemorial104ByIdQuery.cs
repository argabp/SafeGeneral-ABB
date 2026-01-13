using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Application.JurnalMemorials104.Queries
{
    public class GetJurnalMemorial104ByIdQuery : IRequest<JurnalMemorial104Dto>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
    }

    public class GetJurnalMemorial104ByIdQueryHandler : IRequestHandler<GetJurnalMemorial104ByIdQuery, JurnalMemorial104Dto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
        // [TAMBAHAN] Inject UserManager untuk cari nama user
        private readonly UserManager<AppUser> _userManager;

        public GetJurnalMemorial104ByIdQueryHandler(IDbContextPstNota context, IMapper mapper,  UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
             _userManager = userManager;
        }

        public async Task<JurnalMemorial104Dto> Handle(GetJurnalMemorial104ByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Ambil Entity dari Database
            var entity = await _context.JurnalMemorial104
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity == null) return null;

            // 2. Convert ke DTO
            var dto = _mapper.Map<JurnalMemorial104Dto>(entity);

            // 3. LOGIC CARI NAMA USER (Sama seperti Voucher/Piutang)
            
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