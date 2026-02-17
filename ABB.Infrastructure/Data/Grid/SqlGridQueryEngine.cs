using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Application.Common.Grids.Interfaces;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace ABB.Infrastructure.Data.Grid
{
    public class SqlGridQueryEngine : IGridQueryEngine
    {
        private readonly IDbConnectionFactory _db;
        private readonly IDbConnectionCSM _dbConnectionCSM;

        public SqlGridQueryEngine(IDbConnectionFactory db, IDbConnectionCSM dbConnectionCSM)
        {
            _db = db;
            _dbConnectionCSM = dbConnectionCSM;
        }
    
        private string BuildWhereFromFilters(FilterNode node, GridConfig config)
        {
            // leaf
            if (!string.IsNullOrEmpty(node.field))
            {
                if (!config.ColumnMap.ContainsKey(node.field))
                    return string.Empty;


                var col = config.ColumnMap[node.field];
                var val = (node.value ?? "").Replace("'", "''");


                return node.@operator switch
                {
                    "contains" => $"{col} LIKE '%{val}%'",
                    "startswith" => $"{col} LIKE '{val}%'",
                    "endswith" => $"{col} LIKE '%{val}'",
                    "eq" => $"{col} = '{val}'",
                    "neq" => $"{col} <> '{val}'",
                    "gt" => $"{col} > '{val}'",
                    "gte" => $"{col} >= '{val}'",
                    "lt" => $"{col} < '{val}'",
                    "lte" => $"{col} <= '{val}'",
                    _ => string.Empty
                };
            }


            // group
            if (node.filters != null && node.filters.Any())
            {
                var logic = (node.logic ?? "and").ToUpper();


                var parts = node.filters
                    .Select(f => BuildWhereFromFilters(f, config))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();


                if (!parts.Any())
                    return string.Empty;


                return "(" + string.Join($" {logic} ", parts) + ")";
            }


            return string.Empty;
        }

        public async Task<GridResponse<T>> QueryAsync<T>(GridRequest request, GridConfig config, 
            object parameters, string databaseName)
        {
            // DB already selected by CreateDbConnection()
            var where = new StringBuilder(" WHERE " + config.BaseWhere);

            // ---- dynamic filters (json) ----
            if (!string.IsNullOrWhiteSpace(request.FiltersJson))
            {
                var group = JsonConvert.DeserializeObject<FilterGroup>(request.FiltersJson);
                if (group?.filters?.Any() == true)
                {
                    var root = new FilterNode
                    {
                        logic = group.logic,
                        filters = group.filters
                    };

                    var sqlFilter = BuildWhereFromFilters(root, config);
                    if (!string.IsNullOrWhiteSpace(sqlFilter))
                    {
                        where.Append(" AND " + sqlFilter);
                    }
                }
            }
        
            // ---- global search ----
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                var key = request.SearchKeyword.Replace("'", "''");
                var parts = config.SearchableColumns
                    .Select(c => $"{c} LIKE '%{key}%' ");
            
                where.Append(" AND (" + string.Join(" OR ", parts) + ")");
            }
        
            // ---- sorting ----
            var sortCol = config.ColumnMap.ContainsKey(request.SortField ?? "")
                ? config.ColumnMap[request.SortField]
                : config.ColumnMap.Values.First();

            var sortDir = request.SortDir?.ToUpper() == "DESC" ? "DESC" : "ASC";
        
            // ---- paging ----
            int start = ((request.Page - 1) * request.PageSize) + 1;
            int end = request.Page * request.PageSize;
        
            var sql = $@"
            ;WITH Q AS (
            SELECT
            ROW_NUMBER() OVER (ORDER BY {sortCol} {sortDir}) AS rn,
            *
            {config.FromSql}
            {where}
            )
            SELECT * FROM Q WHERE rn BETWEEN {start} AND {end};


        SELECT COUNT(1)
        {config.FromSql}
        {where};
        ";
        
            _db.CreateDbConnection(databaseName);
            using var multi = await _db.QueryMultipleAsync(sql, parameters);
        
            var data = (await multi.ReadAsync<T>()).ToList();
            var total = await multi.ReadFirstAsync<int>();
        
            return new GridResponse<T>
            {
                Data = data,
                Total = total
            };
        }

        public async Task<GridResponse<T>> QueryAsyncCSM<T>(GridRequest request, GridConfig config, 
            object parameters)
            {
            // DB already selected by CreateDbConnection()
            var where = new StringBuilder(" WHERE " + config.BaseWhere);

            // ---- dynamic filters (json) ----
            if (!string.IsNullOrWhiteSpace(request.FiltersJson))
            {
                var group = JsonConvert.DeserializeObject<FilterGroup>(request.FiltersJson);
                if (group?.filters?.Any() == true)
                {
                    var root = new FilterNode
                    {
                        logic = group.logic,
                        filters = group.filters
                    };

                    var sqlFilter = BuildWhereFromFilters(root, config);
                    if (!string.IsNullOrWhiteSpace(sqlFilter))
                    {
                        where.Append(" AND " + sqlFilter);
                    }
                }
            }
        
            // ---- global search ----
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                var key = request.SearchKeyword.Replace("'", "''");
                var parts = config.SearchableColumns
                    .Select(c => $"{c} LIKE '%{key}%' ");
            
                where.Append(" AND (" + string.Join(" OR ", parts) + ")");
            }
        
            // ---- sorting ----
            var sortCol = config.ColumnMap.ContainsKey(request.SortField ?? "")
                ? config.ColumnMap[request.SortField]
                : config.ColumnMap.Values.First();

            var sortDir = request.SortDir?.ToUpper() == "DESC" ? "DESC" : "ASC";
        
            // ---- paging ----
            int start = ((request.Page - 1) * request.PageSize) + 1;
            int end = request.Page * request.PageSize;
        
            var sql = $@"
            ;WITH Q AS (
            SELECT
            ROW_NUMBER() OVER (ORDER BY {sortCol} {sortDir}) AS rn,
            *
            {config.FromSql}
            {where}
            )
            SELECT * FROM Q WHERE rn BETWEEN {start} AND {end};


        SELECT COUNT(1)
        {config.FromSql}
        {where};
        ";
        
            using var multi = await _dbConnectionCSM.QueryMultipleAsync(sql, parameters);
        
            var data = (await multi.ReadAsync<T>()).ToList();
            var total = await multi.ReadFirstAsync<int>();
        
            return new GridResponse<T>
            {
                Data = data,
                Total = total
            };
        }
    }
}
