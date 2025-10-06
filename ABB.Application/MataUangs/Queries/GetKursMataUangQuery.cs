using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.MataUangs.Queries
{
    public class GetKursMataUangQuery : IRequest<decimal>
    {
        public string DatabaseName { get; set; }
        public string KodeMataUang { get; set; }
        public DateTime TanggalVoucher { get; set; }
    }

    public class GetKursMataUangQueryHandler : IRequestHandler<GetKursMataUangQuery, decimal>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetKursMataUangQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<decimal> Handle(GetKursMataUangQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var sql = @"
            SELECT TOP 1 nilai_kurs 
            FROM rf06d
            WHERE kd_mtu = @KodeMataUang 
            AND tgl_mul <= @TanggalVoucher 
            ORDER BY tgl_mul DESC";
            
             var results = await _connectionFactory.Query<DetailMataUangDto>(sql, 
                new { request.KodeMataUang, request.TanggalVoucher });

            // 2. Ambil hasil pertama dari daftar menggunakan LINQ
            var firstResult = results.FirstOrDefault();

            // Logika ini tetap sama dan sudah benar
            return firstResult?.nilai_kurs ?? 1;
        }
    }
}