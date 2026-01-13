using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Application.JurnalMemorial117.Queries
{
    public class GetJurnalMemorial117ByIdQuery : IRequest<JurnalMemorial117Dto>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
    }

    public class GetJurnalMemorial117ByIdQueryHandler : IRequestHandler<GetJurnalMemorial117ByIdQuery, JurnalMemorial117Dto>
    {
        private readonly IDbContextPstNota _context;
        private readonly IMapper _mapper;
         // [TAMBAHAN] Inject UserManager untuk cari nama user
        private readonly UserManager<AppUser> _userManager;


        public GetJurnalMemorial117ByIdQueryHandler(IDbContextPstNota context, IMapper mapper,  UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<JurnalMemorial117Dto> Handle(GetJurnalMemorial117ByIdQuery request, CancellationToken cancellationToken)
        {
          var entity = await _context.JurnalMemorial117
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.KodeCabang == request.KodeCabang && x.NoVoucher == request.NoVoucher, cancellationToken);

            if (entity == null) return null;

            // 2. Map ke DTO
            var dto = _mapper.Map<JurnalMemorial117Dto>(entity);

            // 3. LOGIC CARI NAMA USER (Input)
            if (!string.IsNullOrEmpty(dto.KodeUserInput))
            {
                var userIn = await _userManager.FindByIdAsync(dto.KodeUserInput);
                if (userIn != null) 
                {
                    dto.KodeUserInput = userIn.UserName; 
                }
            }

            // 4. LOGIC CARI NAMA USER (Update)
            if (!string.IsNullOrEmpty(dto.KodeUserUpdate))
            {
                var userUp = await _userManager.FindByIdAsync(dto.KodeUserUpdate);
                if (userUp != null) 
                {
                    dto.KodeUserUpdate = userUp.UserName; 
                }
            }

            return dto;
        }
    }
}