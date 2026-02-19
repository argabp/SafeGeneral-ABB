using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.Application.EntriPembayaranBanks.Queries
{
    public class GetLabaRugiKursQuery : IRequest<string>
    {
        public string gl_dept { get; set; } // Kode Cabang (10, 20, 50)
    }

    public class GetLabaRugiKursQueryHandler : IRequestHandler<GetLabaRugiKursQuery, string>
    {
        private readonly IDbContextPstNota _context;

        public GetLabaRugiKursQueryHandler(IDbContextPstNota context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetLabaRugiKursQuery request, CancellationToken cancellationToken)
        {
            // 1. Konversi string "50" menjadi int 50
            if (!int.TryParse(request.gl_dept, out int deptId))
            {
                return null;
            }

            // 2. Query dengan tipe data yang sama (int vs int)
            var glKodeInt = await _context.LabaRugiKurs
                .Where(x => x.gl_dept == deptId)
                .Select(x => x.gl_kode)
                .FirstOrDefaultAsync(cancellationToken);

            // 3. Kembalikan sebagai string agar bisa masuk ke field KodeAkun di Command
            return glKodeInt != 0 ? glKodeInt.ToString() : null;
        }
    }
}