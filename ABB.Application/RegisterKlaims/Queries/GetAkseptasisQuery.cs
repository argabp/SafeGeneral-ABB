using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetAkseptasisQuery : IRequest<object>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        // paging
        public int Page { get; set; }
        public int PageSize { get; set; }

        // sorting
        public string SortField { get; set; }
        public string SortDir { get; set; }

        // filtering
        public string FilterField { get; set; }
        public string FilterValue { get; set; }
    }

    public class GetAkseptasisQueryHandler : IRequestHandler<GetAkseptasisQuery, object>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetAkseptasisQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<object> Handle(GetAkseptasisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);

            int startRow = ((request.Page - 1) * request.PageSize) + 1;
            int endRow = request.Page * request.PageSize;

            var where = new StringBuilder(@"
                WHERE cb.kd_cb = @KodeCabang
                  AND p.kd_cob = @kd_cob
                  AND p.kd_scob = @kd_scob
            ");

            // 🔍 Global search (any column)
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                var key = request.SearchKeyword.Replace("'", "''");

                where.Append($@"
                    AND (
                        cb.nm_cb      LIKE '%{key}%'
                     OR cob.nm_cob    LIKE '%{key}%'
                     OR scob.nm_scob  LIKE '%{key}%'
                     OR p.no_pol_ttg LIKE '%{key}%'
                     OR p.no_pol_pas LIKE '%{key}%'
                     OR p.nm_ttg     LIKE '%{key}%'
                     OR p.no_rsk     LIKE '%{key}%'
                     OR p.ket_oby    LIKE '%{key}%'
                    )
                ");
            }

            // 🧱 Column filter (single field)
            if (!string.IsNullOrWhiteSpace(request.FilterField) &&
                !string.IsNullOrWhiteSpace(request.FilterValue))
            {
                var col = MapColumn(request.FilterField);
                var val = request.FilterValue.Replace("'", "''");

                where.Append($" AND {col} LIKE '%{val}%'");
            }

            // ↕ Sorting
            var orderBy = "ORDER BY p.no_pol_ttg";
            if (!string.IsNullOrEmpty(request.SortField))
            {
                orderBy = $"ORDER BY {MapColumn(request.SortField)} {(request.SortDir == "desc" ? "DESC" : "ASC")}";
            }

            var sql = $@"
                WITH PagedData AS (
                    SELECT 
                        p.*, 
                        cb.nm_cb, 
                        cob.nm_cob, 
                        scob.nm_scob,
                        ROW_NUMBER() OVER ({orderBy}) AS RowNum
                    FROM v_uw04e p
                    INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                    INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                    INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                        AND p.kd_scob = scob.kd_scob
                    {where}
                )
                SELECT *
                FROM PagedData
                WHERE RowNum BETWEEN @StartRow AND @EndRow;

                SELECT COUNT(1)
                FROM v_uw04e p
                INNER JOIN rf01 cb ON p.kd_cb = cb.kd_cb
                INNER JOIN rf04 cob ON p.kd_cob = cob.kd_cob
                INNER JOIN rf05 scob ON p.kd_cob = scob.kd_cob 
                                    AND p.kd_scob = scob.kd_scob
                {where};
            ";

            using var multi = await _connectionFactory.QueryMultipleAsync(sql, new
            {
                request.KodeCabang,
                request.kd_cob,
                request.kd_scob,
                StartRow = startRow,
                EndRow = endRow
            });

            var data = (await multi.ReadAsync<AkseptasiDto>()).ToList();
            var total = await multi.ReadSingleAsync<int>();

            return new
            {
                Data = data,
                Total = total
            };
        }
        
        private string MapColumn(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
                return "p.no_pol_ttg"; // default sort column

            return field switch
            {
                "nm_cb"        => "cb.nm_cb",
                "nm_cob"       => "cob.nm_cob",
                "nm_scob"      => "scob.nm_scob",
                "no_pol_ttg"   => "p.no_pol_ttg",
                "no_pol_pas"   => "p.no_pol_pas",
                "no_rsk"       => "p.no_rsk",
                "nm_ttg"       => "p.nm_ttg",
                "ket_oby"      => "p.ket_oby",
                "tgl_mul_ptg"  => "p.tgl_mul_ptg",
                "tgl_akh_ptg"  => "p.tgl_akh_ptg",
                "tgl_closing"  => "p.tgl_closing",
                _              => "p.no_pol_ttg"  // fallback safety
            };
        }
    }
}