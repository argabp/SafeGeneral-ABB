using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    public class GetLabaRugiKursQuery : IRequest<string>
    {
        public string gl_dept { get; set; } // Input string dari cookie (contoh: "50")
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
            // 1. Validasi input: Jika kosong, langsung balikkan null
            if (string.IsNullOrEmpty(request.gl_dept)) return null;

            // 2. Konversi string "50" menjadi int 50 (karena kolom gl_dept di DB adalah int)
            if (!int.TryParse(request.gl_dept, out int deptId))
            {
                return null;
            }

            // 3. Query ke tabel abb_kodeakun_valas menggunakan tipe data int
            var glKodeInt = await _context.LabaRugiKurs
                .Where(x => x.gl_dept == deptId)
                .Select(x => x.gl_kode)
                .FirstOrDefaultAsync(cancellationToken);

            // 4. Balikkan gl_kode sebagai string (misal: "70010100") 
            // agar bisa langsung dipakai di field KodeAkun Command
            return glKodeInt != 0 ? glKodeInt.ToString() : null;
        }
    }
}